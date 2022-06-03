using KamalOnLineShop.Data;
using KamalOnLineShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace KamalOnLineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTypesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_context.ProductTypes.ToList());
        }
       

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(ProductTypes model)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Add(model);
              await  _context.SaveChangesAsync();
                TempData["save"] = "Product Type Saved ";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var productType = _context.ProductTypes.Find(id);
            if(productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(ProductTypes model)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Update(model);
              await  _context.SaveChangesAsync();
                TempData["edit"] = "Product Type Updated";
                return RedirectToAction(nameof(Index));

            }
            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _context.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductTypes model)
        {
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productType = _context.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(ProductTypes model)
        {
            if(model != null)
            {
                _context.ProductTypes.Remove(model);
              await  _context.SaveChangesAsync();
                TempData["del"] = "Product type deleted";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
