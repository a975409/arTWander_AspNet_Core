using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;

namespace arTWander.Authentications
{
    public class LoginAuthenticationHandler : IAuthenticationSignInHandler
    {
        public const string LOGIN_SCHEM_NAME = "login_authen";

        public LoginAuthenticationOptions Options { get; private set; }

        public LoginAuthenticationHandler(IOptions<LoginAuthenticationOptions> opt)
        {
            Options = opt.Value;
        }

        public HttpContext HttpContext { get; private set; }
        public AuthenticationScheme Scheme { get; private set; }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            if (Scheme.Name != LOGIN_SCHEM_NAME)
            {
                return Task.FromResult(AuthenticateResult.Fail("驗證方案不匹配"));
            }

            if (!HttpContext.Session.Keys.Any())
            {
                return Task.FromResult(AuthenticateResult.Fail("Session無效"));
            }

            #region 驗證通過
            //以下流程就是

            //要注意指定驗證方案
            ClaimsIdentity id = new ClaimsIdentity(LOGIN_SCHEM_NAME);

            //從Session獲取使用者資訊
            foreach (var key in HttpContext.Session.Keys)
            {
                //要取出多個權限(Roles)，要將其做反序列化的動作
                if (key == ClaimTypes.Role)
                {
                    string[] Roles = JsonSerializer.Deserialize<string[]>(HttpContext.Session.GetString(key));

                    foreach (string item in Roles)
                    {
                        id.AddClaim(new Claim(ClaimTypes.Role, item));
                    }
                }
                else
                {
                    id.AddClaim(new Claim(key, HttpContext.Session.GetString(key)));
                }
            }

            ClaimsPrincipal prcp = new ClaimsPrincipal(id);

            //要注意指定驗證方案
            AuthenticationTicket ticket = new AuthenticationTicket(prcp, LOGIN_SCHEM_NAME);
            return Task.FromResult(AuthenticateResult.Success(ticket));

            #endregion
        }

        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            HttpContext.Response.Redirect(Options.LoginPath);
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties? properties)
        {
            HttpContext.Response.Redirect(Options.ReturnUrlKey);
            return Task.CompletedTask;
        }

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            HttpContext = context;
            Scheme = scheme;
            return Task.CompletedTask;
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            List<string> Roles = new List<string>();

            //將在LoginController的Login設定的使用者資訊(Claims)，都存到session裡面，之後每次的Request只要從Session取出使用者資訊就行
            foreach (var claim in user.Claims)
            {
                if (claim.Type == ClaimTypes.Role)
                {
                    Roles.Add(claim.Value);
                }
                else
                {
                    HttpContext.Session.SetString(claim.Type, claim.Value);
                }
            }

            //以Key=ClaimTypes.Role為例，如果要設定多種權限(Roles)，則要將其轉成JSON字串後再存入Cookie
            string jsonRolesString = JsonSerializer.Serialize(Roles.ToArray());

            HttpContext.Session.SetString(ClaimTypes.Role, jsonRolesString);

            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties? properties)
        {
            //移除存在Session裡面所有使用者資訊
            foreach (var key in HttpContext.Session.Keys)
            {
                HttpContext.Session.Remove(key);
            }

            return Task.CompletedTask;
        }
    }
}
