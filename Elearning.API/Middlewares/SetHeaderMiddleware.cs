//using FirebaseAdmin.Auth;
//using FirebaseAdmin;
//using PersonalBranding.API.Interfaces;
//using System.Globalization;
//using Core.Interfaces;
//using Core.Implements;
//using PersonalBranding.Common.Models;
//using Core;
//using System.Linq;
//using Common.Extensions;
//using Common.Extension;
//using Microsoft.Extensions.Primitives;
//using static Google.Apis.Requests.BatchRequest;
//using System.Security.Claims;
//using Firebase.Auth;

//namespace PersonalBranding.API.Middlewares
//{
//    public class SetHeaderMiddleware
//    {
//        private readonly RequestDelegate _next;
//        IUserBL _userBL;
//        private FirebaseApp _firebaseApp;

//        public SetHeaderMiddleware(FirebaseApp firebaseApp,RequestDelegate next, IUserBL userBL)
//        {
//            _next = next;
//            _userBL = userBL;
//            _firebaseApp = firebaseApp;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            var token = context.Request.Headers["Authorization"];

//            if (!string.IsNullOrWhiteSpace(token))
//            {
//                try
//                {
//                    BaseInfo.email = null;
//                    BaseInfo.name = null;
//                    BaseInfo.UserName = BaseInfo.name;
//                    BaseInfo.firebaseid = null;
//                    BaseInfo.token = null;
//                    BaseInfo.UserName = null;
//                    BaseInfo.UserID = 0;
//                    BaseInfo.IsAdmin = false;

//                    var firebaseToken = await VerifyToken(token);

//                    if (firebaseToken == null)
//                    {
//                        context.Response.Cookies.Append("x-firebase-token", token, new CookieOptions
//                        {
//                            Expires = DateTimeOffset.UtcNow.AddDays(-1)
//                        });
//                    }
//                    else
//                    {
//                        var claims = ConvertUtility.Deserialize<Dictionary<string, object>>(ConvertUtility.Serialize(firebaseToken.Claims));

//                        BaseInfo.email = claims.GetValue<string>("email");
//                        BaseInfo.name = claims.ContainsKey("name") ? claims.GetValue<string>("name") : BaseInfo.email;
//                        BaseInfo.UserName = BaseInfo.name;
//                        BaseInfo.firebaseid = firebaseToken.Uid;
//                        BaseInfo.token = token;

//                        var user = await _userBL.GetUser();

//                        if (user != null)
//                        {
//                            BaseInfo.UserName = user.UserName;
//                            BaseInfo.UserID = user.UserID;
//                            BaseInfo.IsAdmin = user.IsAdmin;
//                        }

//                        if (user?.IsAdmin == true)
//                        {
//                            context.Request.Headers["IsAdmin"] = "true";
//                        }
//                        else context.Request.Headers.Remove("IsAdmin");
//                    }
//                }
//                catch (Exception)
//                {
//                    BaseInfo.token = null;
//                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
//                    await context.Response.WriteAsync("Unauthorized: Missing Authorization or Expire");
//                    return;
//                }

//            }

//            // Call the next delegate/middleware in the pipeline.
//            await _next(context);
//        }

//        private async Task<FirebaseToken> VerifyToken(StringValues token)
//        {
//            try
//            {
//                return await FirebaseAuth.GetAuth(_firebaseApp).VerifyIdTokenAsync(token);
//            }
//            catch (Exception)
//            {
//                return null;
//            }
//        }
//    }
//}
