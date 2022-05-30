using KamalOnLineShop.Data;
using KamalOnLineShop.Models;
using KamalOnLineShop.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KamalOnLineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(Order order)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                foreach (var product in products)
                {
                    OrderDetails orderDetails=new OrderDetails();
                    orderDetails.ProductId = product.Id;
                    //order.OrderDetails = new List<OrderDetails>();
                    order.OrderDetails.Add(orderDetails);
                }
            }
            order.OrderNo=GetOrderNo();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            HttpContext.Session.Set("products",new List<Products>());
            return View();

        }

        public string GetOrderNo()
        {
            int rowCount = _context.Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }
    }
}
