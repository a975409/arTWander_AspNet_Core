using arTWander.Authentications;
using arTWander.Data;
using arTWander.Models;
using arTWander.Models.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;

namespace arTWander.Controllers
{
	[Route("[controller]")]
	public class LoginController : Controller
	{
		private readonly ApplicationContext _context;

		public LoginController(ApplicationContext context)
		{
			_context = context;
		}

		[Route("~/")]
		[Route("Index")]
		public IActionResult Index()
		{
			return View();
		}

        [HttpGet("Register")]
		public IActionResult Detail()
		{
			return View();
		}

        [ValidateAntiForgeryToken]
        [HttpPost("Register")]
        public IActionResult Detail(RegisterUser user)
        {
			if (!ModelState.IsValid)
				return View(user);
			
			var result = new User
			{
				Email = user.Email,
				Password = user.Password.ToMD5(),
				Name = user.Name,
				RoleId = 2,
				Birthday = user.Birthday,
				PhoneNumber = user.PhoneNumber,
				Picture = user.Picture,
				UserName = user.UserName
			};

			_context.Users.Add(result);
			_context.SaveChanges();
            TempData["Success"] = true;
			
			return RedirectToAction("Index");
        }

        [HttpPost("api/UserLogin")]
		public async Task<IActionResult> UserLogin([FromBody]UserLogin value)
		{
			var result = _context.Users.Where(m => m.Email == value.Email && m.Password == value.Password.ToMD5()).FirstOrDefault();

			if (result == null)
				return StatusCode(401, "登入失敗，帳號密碼錯誤");

			//建立使用者資訊
			var claims = new List<Claim> {
					 new Claim(ClaimTypes.Email,result.Email)//使用者名稱
					};

			//取得權限(Roles)
			var Roles = _context.Roles.Where(m => m.RoleId == result.RoleId);

			//新增權限(Roles)
			foreach (var role in Roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role.Name));
			}

			// 要注意指定驗證方案
			var claimsIdentity = new ClaimsIdentity(claims, LoginAuthenticationHandler.LOGIN_SCHEM_NAME);
			await HttpContext.SignInAsync(LoginAuthenticationHandler.LOGIN_SCHEM_NAME, new ClaimsPrincipal(claimsIdentity));
			return Ok("登入成功");
		}

		[HttpGet("api/CheckEmail")]
		public async Task<IActionResult> CheckEmail([Bind("Email"), FromQuery] RegisterUser registerUser)
		{
			if (ModelState["Email"].ValidationState != ModelValidationState.Valid)
			{
				string errorMsg = ModelState["Email"].Errors[0].ErrorMessage;
				return Ok(errorMsg);
			}

			var result = _context.Users.Where(m => m.Email == registerUser.Email).FirstOrDefault();

			if (result != null)
				return Ok("此Email已被註冊!!");
			else
				return Ok("");
		}

		[HttpGet("api/CheckPassword")]
		public async Task<IActionResult> CheckPassword([Bind("Password"), FromQuery] RegisterUser registerUser)
		{
			if (ModelState["Password"].ValidationState != ModelValidationState.Valid)
			{
				string errorMsg = ModelState["Password"].Errors[0].ErrorMessage;
				return Ok(errorMsg);
			}
			else
				return Ok("");
		}
	}
}
