using System.ComponentModel.DataAnnotations;

namespace arTWander.Models.Dtos
{
    public class UserLogin
    {
        [Display(Name = "電子信箱")]
        [Required(ErrorMessage = "該欄位必填")]
        [EmailAddress(ErrorMessage = "請輸入正確的Email")]
        public string Email { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "該欄位必填")]
        public string Password { get; set; }
    }
}
