using Common.Extension;
using Core;
using Core.Enums;
using Core.Implements;
using Core.Models;
using DatabaseService.Interfaces;
using Elearning.BL.Interfaces;
using Elearning.Models;
using EmailService.Interfaces;
using EmailService.Models;
using FirebaseService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Elearning.BL.Implements
{
    public class CheckoutBL : ICheckoutBL
    {
        IUserBL _userBL;
        IDatabaseService _databaseService;
        IFirebaseAuthService _firebaseService;
        private static Random random = new Random();
        IHttpContextAccessor _httpContextAccessor;
        IConfigurationSection _appsetting;
        IEmailService _emailService;

        public CheckoutBL(IConfiguration configuration,IEmailService emailService, IUserBL userBL, IDatabaseService databaseService, IFirebaseAuthService firebaseAuthService, IHttpContextAccessor httpContextAccessor, ICoursePackageDetailBL coursePackageDetail)
        {
            _userBL = userBL;
            _databaseService = databaseService;
            _firebaseService = firebaseAuthService;
            _httpContextAccessor = httpContextAccessor;
            _appsetting = configuration.GetSection("AppSettings");
            _emailService = emailService;
        }

        public async Task<ServiceResponse> Checkout(Dictionary<string, object> payload)
        {
            var result = new ServiceResponse();
            result.Success = false;

            var coursePackageID = payload.GetValue<int?>("CoursePackageID");
            var promotionID = payload.GetValue<int?>("PromotionID");
            var receiverEmail = payload.GetValue<string?>("ReceiverEmail");
            var referrerID = payload.GetValue<string>("ReferrerID");

            var validateResults = ValidatePayload(coursePackageID, receiverEmail);

            if (validateResults.Count > 0)
            {
                result.ValidateInfo = validateResults;
                return result;
            }

            var checkoutInfo = await _databaseService.QuerySingleUsingStoredProcedureAsync<object>("Proc_GetCheckoutInfo", new Dictionary<string, object>
            {
                {"v_CoursePackageID", coursePackageID ?? 0 },
                {"v_ReceiverEmail", receiverEmail ?? "" },
                {"v_PromotionID", promotionID ?? 0 },
            });

            if (checkoutInfo == null) return result;
            ValidateCheckoutInfo(coursePackageID, promotionID, validateResults, checkoutInfo);

            if (validateResults.Count > 0)
            {
                result.ValidateInfo = validateResults;
                return result;
            }

            // Lấy thông tin người dùng
            // Chưa có người dùng
            var password = GenerateStrongPassword();
            var userID = checkoutInfo.GetValue<int?>("UserID");
            userID = await HandleUserID(receiverEmail, password, userID);
            var coursePackageDetailID = checkoutInfo.GetValue<int?>("CoursePackageDetailID");

            // Build CoursePackageDetail
            var param = new Dictionary<string, object>
            {
                {"v_CourseID", checkoutInfo.GetValue<int>("CourseID")},
                {"v_CoursePackageDetailID", coursePackageDetailID ?? 0},
                {"v_NextCoursePackageID", coursePackageID ?? 0},
                {"v_NextPurchasePrice", checkoutInfo.GetValue<decimal>("PurchasePrice")},
                {"v_ReceiverEmail", receiverEmail ?? ""},
                {"v_UserID", userID ?? 0},
                {"v_NextPrice", checkoutInfo.GetValue<decimal>("Price")},
                {"v_ReferrerID", referrerID ?? ""},
                {"v_Password", password }
            };

            var ordercoder = await _databaseService.ExecuteScalarUsingStoreProcedureAsync<string>("Proc_CoursePackageDetail_Checkout", param);

            result.Success = true;
            result.Data = new
            {
                OrderCode = ordercoder,
                PurchasePrice = checkoutInfo.GetValue<decimal>("PurchasePrice")
            };

            return result;
        }

        private static void ValidateCheckoutInfo(int? coursePackageID, int? promotionID, List<ValidateResult> validateResults, object checkoutInfo)
        {
            // Lấy thông tin gói và khóa học
            // Validate xem gói có tồn tại và khóa học có tồn tại không
            if (checkoutInfo.GetValue<int?>("CourseID") == null) validateResults.Add(new ValidateResult
            {
                ErrorCode = ErrorCodeEnum.NotFound,
                FieldError = "CoursePackageID",
                FieldValue = coursePackageID ?? 0,
            });

            // Kiểm tra nâng cấp gói có đúng không
            if (checkoutInfo.GetValue<int?>("CoursePackageDetailID") != null && checkoutInfo.GetValue<bool?>("IsUpgradeValid") == false)
                validateResults.Add(new ValidateResult
                {
                    ErrorCode = ErrorCodeEnum.InvalidUpgrade,
                    FieldError = "CoursePackageID",
                    FieldValue = coursePackageID ?? 0,
                });

            // Lấy thông tin giảm giá
            // Validate mã giảm giá có không và đã hết hạn chưa
            if (promotionID != null && checkoutInfo.GetValue("PromotionValue") == null)
                validateResults.Add(new ValidateResult
                {
                    ErrorCode = ErrorCodeEnum.NotFound,
                    FieldError = "PromotionID",
                    FieldValue = promotionID ?? 0,
                });

            if (promotionID != null && checkoutInfo.GetValue<int?>("IsExpiredPromotion") == 1)
                validateResults.Add(new ValidateResult
                {
                    ErrorCode = ErrorCodeEnum.Expired,
                    FieldError = "PromotionID",
                    FieldValue = promotionID ?? 0,
                });
        }

        private async Task<int?> HandleUserID(string? receiverEmail, string password, int? userID)
        {
            try
            {
                if (userID == null)
                {
                    var userCrendentials = await _firebaseService.CreateUserWithEmailAndPasswordAsync(receiverEmail, password);
                    var userInfo = userCrendentials.User.Info;
                    var newUser = new Models.User();
                    newUser.UserID = 0;
                    newUser.Email = userInfo.Email;
                    newUser.FullName = userInfo.DisplayName;
                    newUser.FirebaseID = userInfo.Uid;
                    newUser.IsAdmin = false;
                    newUser.State = DatabaseService.Enums.ModelStateEnum.Insert;
                    newUser.Password = password;

                    var res = await _userBL.Save(newUser);
                    if (res != null && res.Success) userID = newUser.UserID;
                }

                return userID;
            }
            catch (Exception)
            {
                throw new Exception("Có lỗi trong quá trình tạo tài khoản");
            }
        }

        private static List<ValidateResult> ValidatePayload(int? coursePackageID, string? receiverEmail)
        {
            var validateResults = new List<ValidateResult>();
            if (string.IsNullOrWhiteSpace(receiverEmail) || !receiverEmail.ValidateEmail())
                validateResults.Add(new ValidateResult
                {
                    ErrorCode = ErrorCodeEnum.EmailInValid,
                    FieldError = "ReceiverEmail",
                    FieldValue = receiverEmail ?? "",
                });

            if (coursePackageID == null)
                validateResults.Add(new ValidateResult
                {
                    ErrorCode = ErrorCodeEnum.ValueInvalid,
                    FieldError = "CoursePackageID",
                    FieldValue = coursePackageID ?? 0,
                });
            return validateResults;
        }

        public static string GenerateStrongPassword(int length = 8)
        {
            if (length < 4)
                throw new ArgumentException("Password length must be at least 4 to include all character types.");

            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string special = "!@#$%^&*";

            // Chọn ít nhất 1 ký tự từ mỗi nhóm
            var passwordChars = new List<char>
            {
                lower[random.Next(lower.Length)],
                upper[random.Next(upper.Length)],
                digits[random.Next(digits.Length)],
                special[random.Next(special.Length)]
            };

            // Tập hợp tất cả ký tự lại
            string allChars = lower + upper + digits + special;

            // Chọn ngẫu nhiên các ký tự còn lại
            for (int i = passwordChars.Count; i < length; i++)
            {
                passwordChars.Add(allChars[random.Next(allChars.Length)]);
            }

            // Xáo trộn mật khẩu để ngẫu nhiên vị trí
            return new string(passwordChars.OrderBy(x => random.Next()).ToArray());
        }

        async Task<ServiceResponse> ICheckoutBL.ReceiveCheckoutResult(Dictionary<string, object> param)
        {
            var result = new ServiceResponse();
            result.Success = false;
            var content = param.GetValue<string>("content");
            var money = param.GetValue<int>("money");
            var type = param.GetValue<string>("type");
            var tokenHeader = _httpContextAccessor.HttpContext?.Request.Headers["X-Thueapi"].ToString();
            var tokerConfig = _appsetting["ThueAPIToken"];

            if (tokenHeader == tokerConfig)
            {
                // Lưu thông tin thanh toán
                _ = SaveCheckoutInfo(content, money, type);

                // Lấy couse_package_detail
                var paramObject = new Dictionary<string, object>
                {
                    {"v_Content", content },
                    {"v_Money", money }
                };
                var coursePackageDetail = await _databaseService.QuerySingleUsingCommanTextAsync<CoursePackageDetail>("SELECT * FROM course_package_detail cpd WHERE cpd.OrderCode = @Content AND cpd.NextPurchasePrice = @Money;", paramObject);
                var rowEffect = await _databaseService.ExecuteUsingStoredProcedureAsync("Proc_course_package_detail_update_after_checkout", paramObject);

                if (rowEffect == 0) return result;

                // Lưu thông tin cho người giới thiệu
                _ = SaveReferrerInfo(content, coursePackageDetail);

                if (coursePackageDetail == null) return result;

                // Gửi mail cho người dùng
                SendMailCheckoutSuccess(coursePackageDetail);
            }

            return result;
        }

        private void SendMailCheckoutSuccess(CoursePackageDetail coursePackageDetail)
        {
            var emailContent = "<div style=\"display: flex;flex-direction: column;align-items: center;\">\r\n    <div style=\"display: flex;flex-direction: column;align-items: center;width: 480px;\">\r\n        <div style=\"font-size: 20px;font-weight: 700;\">CHÀO MỪNG ĐẾN VỚI FUTURCREATORS</div>\r\n        <br>\r\n        <br>\r\n        <div>##UserName##, Đây Là Quyền Truy Cập của Ông/Bà Vào Chương Trình:</div>\r\n        <br>\r\n        <br>\r\n        <div>\r\n            <div>Email: ##UserEmail##</div>\r\n            <div>Mật khẩu: ##UserPassword##</div>\r\n        </div>\r\n        <br>\r\n        <br>\r\n        <div style=\"display: flex;flex-direction: column;align-items: center;text-align: center;\">\r\n            <div style=\"width: 240px;height: 40px;background: #2a7dff;border-radius: 4px;display: flex;align-items: center;justify-content: center;color: white;font-weight: 600;\">\r\n                <a style=\"text-decoration: none;color: white;\" href=\"##LinkWebsite##\">ĐĂNG NHẬP NGAY</a></div>\r\n            <br>\r\n            <div>\"Nếu ông/bà đã có tài khoản với địa chỉ email tương tự, hãy nhập mật khẩu cá nhân của ông/bà hoặc thử đặt lại mật khẩu.\"</div>\r\n        </div>\r\n        <br>\r\n        <br>\r\n        <hr style=\"width: 100%;\">\r\n        <div style=\"width: 100%;padding-left: 48px;padding-top: 12px;\">\r\n            <div>Kenvin Mach</div>\r\n            <div>Founder</div>\r\n            <div>support@futurcreators.com</div>\r\n            <div>https://www.instagram.com/futurcreators</div>\r\n            <div>https://www.facebook.com/futurcreators</div>\r\n        </div>\r\n    </div>\r\n</div>";

            emailContent = emailContent.Replace("##UserName##", coursePackageDetail.ReceiverEmail)
                                        .Replace("##UserEmail##", coursePackageDetail.ReceiverEmail)
                                        .Replace("##UserPassword##", coursePackageDetail.Password)
                                        .Replace("##LinkWebsite##", _appsetting["LinkWebsite"]);

            _ = _emailService.SendMail(new EmailService.Models.MailConfig()
            {
                DisplayName = _appsetting["DisplayName"] ?? "Elearning",
                Email = _appsetting["Email"] ?? "Elearning",
                Host = _appsetting["Host"] ?? "Elearning",
                Password = _appsetting["Password"] ?? "Elearning",
                Port = int.Parse(_appsetting["DisplayName"] ?? "465")
            }, new EmailService.Models.MailModel()
            {
                Body = emailContent,
                Subject = "Thông báo mua khóa học thành công",
                To = new List<string> { { coursePackageDetail.ReceiverEmail } }
            });
        }

        private async Task SaveReferrerInfo(string content, CoursePackageDetail coursePackageDetail)
        {
            if (!string.IsNullOrWhiteSpace(coursePackageDetail.ReferrerID?.ToString()))
            {
                var sql = "INSERT INTO order_referrer (Money, FirebaseID) VALUES (@Money, @FirebaseID);";
                await _databaseService.ExecuteUsingCommandTextAsync(sql, new Dictionary<string, object>
                    {
                        {"@Money", content },
                        {"@FirebaseID", coursePackageDetail.ReferrerID }
                    });
            }
        }

        private async Task SaveCheckoutInfo(string content, int money, string type)
        {
            try
            {
                var sql = "INSERT INTO checkout_info (Content, Money, Type) VALUES(@Content, @Money, @Type);";
                await _databaseService.ExecuteUsingCommandTextAsync(sql, new Dictionary<string, object>
                    {
                        {"@Content", content },
                        {"@Money", money },
                        {"@Type", type }
                    });
            }
            catch (Exception)
            {
            }
        }
    }
}
