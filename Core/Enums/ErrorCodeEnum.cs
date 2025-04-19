using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum ErrorCodeEnum
    {
        NoError,
        /// <summary>
        /// Dữ liệu đã được sử dụng
        /// </summary>
        IsUsed,
        /// <summary>
        /// Email không hợp lệ
        /// </summary>
        EmailInValid,
        /// <summary>
        /// Số điện thoại không hợp lệ
        /// </summary>
        PhoneInValid,
        /// <summary>
        /// Trùng dữ liệu
        /// </summary>
        UniqueInValid,
        /// <summary>
        /// Quá dài
        /// </summary>
        MaxLengthInValid,
        /// <summary>
        /// Thiếu dữ liệu
        /// </summary>
        RequiredInValid,
        /// <summary>
        /// Đã tồn tại
        /// </summary>
        Exist,
        /// <summary>
        /// Số lượng sản phẩm không đủ
        /// </summary>
        ProductNotEnough,
        VoucherNotEnough,
        NoPermission,
        InvalidMailConfig,
        InvalidaProductType,
        ProductInBundle,
        SubscribePending,
        SubscribeVerified,
    }
}
