using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace arTWander.Models;

public partial class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    
    [Display(Name = "電子信箱")]
    [Required(ErrorMessage = "該欄位必填")]
    [EmailAddress(ErrorMessage = "請輸入正確的Email")]
    public string Email { get; set; }

    [Display(Name = "密碼")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "密碼長度最少8碼，至少一個大寫字母，一個小寫字母和一個數字")]
    [MinLength(8, ErrorMessage = "密碼長度必須8碼或以上")]
    [Required(ErrorMessage = "該欄位必填")]
    public string Password { get; set; }

    [Display(Name = "電子信箱是否驗證")]
    public bool? EmailConfirmed { get; set; }

    [Display(Name = "會員暱稱")]
    public string? UserName { get; set; }

    [Display(Name = "真實姓名")]
    public string? Name { get; set; }

    [Display(Name = "生日")]
    public DateTime? Birthday { get; set; }

    [Display(Name = "連絡電話")]
    [Phone(ErrorMessage ="請輸入正確的連絡電話")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "照片上傳")]
    public string? Picture { get; set; }

    [Display(Name = "啟用二階段驗證")]
    public bool? TwoFactorEnabled { get; set; }

    [Display(Name = "鎖定時間")]
    public DateTime? LockoutEndDateTime { get; set; }

    [Display(Name = "帳戶鎖定")]
    public bool? LockoutEnabled { get; set; }

    [Display(Name = "最後登入時間")]
    public DateTime? LastLoginDateTime { get; set; }

    [Display(Name = "登入失敗次數")]
    public int? AccessFailedCount { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; }
}
