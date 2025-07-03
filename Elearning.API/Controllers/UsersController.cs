using Elearning.BL.Interfaces;
using Elearning.Models;
using Core.Controllers;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using FirebaseService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Core;
using Elearning.BL.Implements;
using Microsoft.AspNetCore.Authorization;

namespace Elearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController<Elearning.Models.User>
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        IUserBL _userBL;

        public UsersController(IBaseBL<Elearning.Models.User> baseBL, IFirebaseAuthService firebaseAuthService, IUserBL userBL) : base(baseBL)
        {
            _firebaseAuthService = firebaseAuthService;
            _userBL = userBL;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignInWithEmailAndPassword([FromBody] Dictionary<string, object> payload)
        {
            if (payload == null || !payload.ContainsKey("email") || !payload.ContainsKey("password"))
            {
                return BadRequest("Email và mật khẩu là bắt buộc.");
            }

            string email = payload["email"] as string;
            string password = payload["password"] as string;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email và mật khẩu không được để trống.");
            }

            try
            {
                string idToken = await (await _firebaseAuthService.SignInWithEmailAndPasswordAsync(email, password))?.User?.GetIdTokenAsync();
                return Ok(idToken);
            }
            catch (FirebaseAuthException ex)
            {
                return Unauthorized("Email hoặc mật khẩu không hợp lệ.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server.");
            }
        }

        [Authorize]
        [HttpGet("user-info")]
        public async Task<ServiceResponse> GetByFirebaseID()
        {
            var result = new ServiceResponse();

            if (string.IsNullOrWhiteSpace(BaseInfo.firebaseid)) return result;

            result.Data = await _userBL.GetUser(BaseInfo.firebaseid);
            if (result.Data == null)
            {
                var newUser = new Models.User();
                newUser.UserID = 0;
                newUser.Email = BaseInfo.email;
                newUser.FullName = BaseInfo.name;
                newUser.FirebaseID = BaseInfo.firebaseid;
                newUser.IsAdmin = false;
                newUser.State = DatabaseService.Enums.ModelStateEnum.Insert;
                var res = await Save(newUser);
                if (res.Success) result.Data = newUser; else result.Success = false;
            };

            return result;
        }
    }
}
