using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KamalOnLineShop.Models
{
    public class Products
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]

        public decimal Price { get; set; }
        public string Image { get; set; }
        [Required]
        [Display(Name = "Product Color")]

        public string ProductColor { get; set; }
        [Required]
        [Display(Name = "Available")]

        public bool Isavailable { get; set; }
        [Display(Name = "Product Type")]

        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public ProductTypes ProductTypes { get; set; }
        [Display(Name = "Special Tag")]

        public int SpecialTagId { get; set; }
        [ForeignKey("SpecialTagId")]

        public SpecialTag SpecialTag { get; set; }
    }
}
