using KamalOnLineShop.Data;
using KamalOnLineShop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KamalOnLineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IHostingEnvironment _Host { get; }

        public ProductsController(ApplicationDbContext context,IHostingEnvironment Host)
        {
            _context = context;
            _Host = Host;
        }
        public IActionResult Index()
        {
            return View(_context.Products.Include(x=>x.ProductTypes).Include(x=>x.SpecialTag).ToList());
        }

        [HttpPost]
        public IActionResult Index(decimal? LowAmount,decimal? HighAmount)
        {
            var products = _context.Products.Include(x => x.ProductTypes)
                .Include(x => x.SpecialTag).Where(x => x.Price >= LowAmount && x.Price <= HighAmount).ToList();
            if (LowAmount == null || HighAmount == null)
            {
                products = _context.Products.Include(x => x.ProductTypes).Include(x => x.SpecialTag).ToList();
            }
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.ProductTypes = _context.ProductTypes.ToList();
            ViewBag.SpecialTags = _context.SpecialTags.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products model,IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var ExistName=_context.Products.FirstOrDefault(n=>n.Name==model.Name);
                if (ExistName != null)
                {
                    ViewBag.message = "this Product Already Existing";
                    ViewBag.ProductTypes = _context.ProductTypes.ToList();
                    ViewBag.SpecialTags = _context.SpecialTags.ToList();
                    return View(model);
                }
                if(image != null)
                {
                    var name = Path.Combine(_Host.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    model.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    model.Image = "Images/Default.png";
                }
                _context.Products.Add(model);
               await _context.SaveChangesAsync();
                TempData["save"] = "Product saved Succesufully";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ProductTypes = _context.ProductTypes.ToList();
            ViewBag.SpecialTags = _context.SpecialTags.ToList();
            return View(model);

        }

        public IActionResult Edit(int? id)
        {
            ViewBag.ProductTypes = _context.ProductTypes.ToList();
            ViewBag.SpecialTags = _context.SpecialTags.ToList();

            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(x=>x.ProductTypes).Include(x=>x.SpecialTag).FirstOrDefault(x => x.Id == id);
           if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Products model, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_Host.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    model.Image = "Images/" + image.FileName;
                }
                if (image == null)
                {
                    model.Image = "Images/Default.png";
                }
                _context.Products.Update(model);
                await _context.SaveChangesAsync();
                TempData["edit"] = "Product Updated Succesufully";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ProductTypes = _context.ProductTypes.ToList();
            ViewBag.SpecialTags = _context.SpecialTags.ToList();
            return View(model);

        }

        public IActionResult Details(int? id)
        {
            ViewBag.ProductTypes = _context.ProductTypes.ToList();
            ViewBag.SpecialTags = _context.SpecialTags.ToList();

            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(x => x.ProductTypes).Include(x => x.SpecialTag).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Delete(int? id)
        {
            ViewBag.ProductTypes = _context.ProductTypes.ToList();
            ViewBag.SpecialTags = _context.SpecialTags.ToList();

            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(x => x.ProductTypes).Include(x => x.SpecialTag).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Products model)
        {
            if(model != null)
            {
                _context.Products.Remove(model);
               await _context.SaveChangesAsync();
                TempData["del"] = "Product Deledted Succesufully";
                return RedirectToAction(nameof(Index));
            }
            return View(model);

        }

    }
}
