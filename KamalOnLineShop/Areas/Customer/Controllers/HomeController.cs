using KamalOnLineShop.Data;
using KamalOnLineShop.Models;
using KamalOnLineShop.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KamalOnLineShop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
           _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Products.Include(x=>x.ProductTypes).Include(x=>x.SpecialTag).ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(x=>x.ProductTypes).FirstOrDefault(x => x.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            ViewBag.ProductTypes = _context.ProductTypes.ToList();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public IActionResult ProductDetails(int? id)
        {
            List<Products>products=new List<Products>();
            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(x => x.ProductTypes).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.ProductTypes = _context.ProductTypes.ToList();
            products = HttpContext.Session.Get<List<Products>>("products");
            if(products== null)
            {
                 products = new List<Products>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(x => x.Id == id);
                products.Remove(product);
                HttpContext.Session.Set("products", products);

            }
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [ActionName("Remove")]
        public IActionResult RemoveFromCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products != null)
            {
                var product=products.FirstOrDefault(x => x.Id == id);
                products.Remove(product);
                HttpContext.Session.Set("products", products);

            }
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products == null)
            {
                products=new List<Products>();
            }

            return View(products);

        }

    }
}
