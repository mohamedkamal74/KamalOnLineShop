using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KamalOnLineShop.Models
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetails>();
        }
        public int Id { get; set; }
        [Display(Name ="order number")]
        public string OrderNo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "phone number")]

        public string PhoneNo { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]

        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; }

    }
}
