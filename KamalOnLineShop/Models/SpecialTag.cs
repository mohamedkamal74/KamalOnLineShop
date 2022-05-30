using System.ComponentModel.DataAnnotations;

namespace KamalOnLineShop.Models
{
    public class SpecialTag
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tag Name")]
        public string TagName { get; set; }
    }
}
