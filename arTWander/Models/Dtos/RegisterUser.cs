using System.ComponentModel.DataAnnotations;

namespace arTWander.Models.Dtos
{
    public class RegisterUser
    {
        [Display(Name = "電子信箱")]
        [Required(ErrorMessage = "該欄位必填")]
        [EmailAddress(ErrorMessage = "請輸入正確的Email")]
        public string Email { get; set; }

        [Display(Name = "密碼")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "密碼長度最少8碼，至少一個大寫字母，一個小寫字母和一個數字")]
        [MinLength(8, ErrorMessage = "密碼長度必須8碼或以上")]
        [Required(ErrorMessage = "該欄位必填")]
        public string Password { get; set; }

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "該欄位必填")]
        [Compare("Password", ErrorMessage = "與輸入的密碼不一致")]
        public string PasswordConfirmed { get; set; }

        [Display(Name = "會員暱稱")]
        public string? UserName { get; set; }

        [Display(Name = "真實姓名")]
        public string? Name { get; set; }

        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "連絡電話")]
        [Phone(ErrorMessage = "請輸入正確的連絡電話")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "照片上傳")]
        public string? Picture { get; set; }

    }
}
