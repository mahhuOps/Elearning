using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Common.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FirebaseService.Middlewares
{
    public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private FirebaseApp _firebaseApp;

        public FirebaseAuthenticationHandler(FirebaseApp firebaseApp, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _firebaseApp = firebaseApp;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Context.Request.Headers.ContainsKey("Authorization"))
            {
                try
                {
                    var isAdmin = Context.Request.Headers["IsAdmin"].ToString().ToLower() == "true";

                    var listClaims = new List<ClaimsIdentity>() {
                        new ClaimsIdentity(
                            ToClaims(isAdmin),
                            nameof(FirebaseAuthenticationHandler)
                        )
                    };
                    var claimsPrincipal = new ClaimsPrincipal(listClaims);
                    var firebaseTicket = new AuthenticationTicket(claimsPrincipal, JwtBearerDefaults.AuthenticationScheme);
                    return AuthenticateResult.Success(firebaseTicket);
                }
                catch (Exception ex)
                {
                    AuthenticateResult.Fail(ex);
                }
            }

            return AuthenticateResult.NoResult();
        }

        private List<Claim> ToClaims(bool isAdmin)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Role, isAdmin ? RoleType.ADMIN : RoleType.CUSTOMER)
            };

        }
    }
}
