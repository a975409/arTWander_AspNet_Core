using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace arTWander.Models
{
    public partial class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Display(Name = "權限名稱")]
        [Required(ErrorMessage = "該欄位必填")]
        public string Name { get; set; }


        [Display(Name = "啟用權限")]
        public bool Active { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
