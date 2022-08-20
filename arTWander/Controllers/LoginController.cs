using arTWander.Authentications;
using arTWander.Data;
using arTWander.Helpers;
using arTWander.Models;
using arTWander.Models.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;

namespace arTWander.Controllers
{
    [ApiController,Route("[controller]")]
	public class LoginController : Controller
	{
		private readonly ApplicationContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public LoginController(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}

		[Route("~/")]
		[Route("Index")]
		public IActionResult Index()
		{
			return View();
		}

		[Route("Detail")]
		public IActionResult Detail()
		{
			return View();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="registerUser"></param>
		/// <param name="file">檔案上傳：https://docs.microsoft.com/zh-tw/aspnet/core/mvc/models/file-uploads?view=aspnetcore-6.0#file-upload-scenarios</param>
		/// 如果API會接受上傳檔案和其他參數，記得參數的部分要標記FromForm，因為前端axios.post會在header標記 "Content-Type": "multipart/form-data"
		/// https://blog.csdn.net/qq994877603/article/details/112281681
		/// <returns></returns>
		[HttpPost("api/Register")]
		public IActionResult Register([FromForm] RegisterUser registerUser, [UploadFileValid] IFormFile? picfile)
		{
			var user = new User
			{
				Email = registerUser.Email,
				Password = registerUser.Password.ToMD5(),
				PasswordConfirmed = registerUser.PasswordConfirmed,
				UserName = registerUser.UserName,
				Name = registerUser.Name,
				Birthday = registerUser.Birthday,
				PhoneNumber = registerUser.PhoneNumber
			};

			if (picfile != null && picfile.Length > 0)
			{
				using (var ms = new MemoryStream())
				{
					picfile.CopyTo(ms);
					user.Picture = ms.ToArray();
				}
			}

			user.RoleId = 2;
			_context.Users.Add(user);

			try
			{
				_context.SaveChanges();
				return Ok(user.UserId);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

        [HttpPost("api/UserLogin")]
		public async Task<IActionResult> UserLogin(UserLogin value)
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

		/// <summary>
		/// 僅用來驗證輸入參數是否符合規範
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
        [HttpPost("api/CheckInput")]
        public IActionResult CheckInput(RegisterUser user)
        {
			return Ok();
        }
    }
}
