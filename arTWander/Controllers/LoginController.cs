using arTWander.Authentications;
using arTWander.Data;
using arTWander.Models;
using arTWander.Models.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace arTWander.Controllers
{
    [ApiController,Route("[controller]")]
    public class LoginController : Controller
    {
		private readonly ApplicationContext _context;

		public LoginController(ApplicationContext context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
            return View();
        }

		[HttpPost("api/UserLogin")]
		public async Task<IActionResult> UserLogin(UserLogin value)
		{
			var result = _context.Users.Where(m => m.Email == value.Email && m.Password == value.Password).FirstOrDefault();

			if (result == null)
				return StatusCode(401, "登入失敗，帳號密碼錯誤");

			//建立使用者資訊
			var claims = new List<Claim> {
					 new Claim(ClaimTypes.Email,result.Email),//使用者名稱
					};

			//取得權限(Roles)
			//var Roles = _todoContext.Roles.Where(m => m.EmployeeId == result.EmployeeId);

			//新增權限(Roles)
			//foreach (var role in Roles)
			//{
			//	claims.Add(new Claim(ClaimTypes.Role, role.Name));
			//}

			// 要注意指定驗證方案
			var claimsIdentity = new ClaimsIdentity(claims, LoginAuthenticationHandler.LOGIN_SCHEM_NAME);
			await HttpContext.SignInAsync(LoginAuthenticationHandler.LOGIN_SCHEM_NAME, new ClaimsPrincipal(claimsIdentity));
			return Ok("登入成功");
		}
	}
}
