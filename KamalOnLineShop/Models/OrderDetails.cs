using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KamalOnLineShop.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [Display(Name = "order")]

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [Display(Name = "product")]

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]

        public Products Products { get; set; }
    }
}
