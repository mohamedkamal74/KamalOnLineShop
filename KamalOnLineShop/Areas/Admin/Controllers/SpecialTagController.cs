using KamalOnLineShop.Data;
using KamalOnLineShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace KamalOnLineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpecialTagController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.SpecialTags.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(SpecialTag model)
        {
            if (ModelState.IsValid)
            {
                _context.SpecialTags.Add(model);
                await _context.SaveChangesAsync();
                TempData["save"] = "Special Tag Saved";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _context.SpecialTags.FirstOrDefault(x=>x.Id==id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(SpecialTag model)
        {
            if (ModelState.IsValid)
            {
                _context.SpecialTags.Update(model);
                await _context.SaveChangesAsync();
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
            var specialTag = _context.SpecialTags.FirstOrDefault(x=>x.Id==id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
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
            var specialTag = _context.SpecialTags.FirstOrDefault(x=>x.Id==id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(SpecialTag model)
        {
            if (model != null)
            {
                _context.SpecialTags.Remove(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
