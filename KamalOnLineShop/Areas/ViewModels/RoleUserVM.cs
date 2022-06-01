using System.ComponentModel.DataAnnotations;

namespace KamalOnLineShop.Areas.ViewModels
{
    public class RoleUserVM
    {
        [Required]
        [Display(Name ="User")]
        public string  UserId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string  RoleId { get; set; }
    }
}
