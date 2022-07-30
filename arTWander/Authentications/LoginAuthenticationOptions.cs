namespace arTWander.Authentications
{
    public class LoginAuthenticationOptions
    {
		/// <summary>
		/// 未登入時會自動導到這個網址
		/// </summary>
		public string LoginPath { get; set; } = "/Login/Index";

		/// <summary>
		/// 沒有權限時會自動導到這個網址
		/// </summary>
		public string ReturnUrlKey { set; get; } = "/Login/Index";
	}
}
