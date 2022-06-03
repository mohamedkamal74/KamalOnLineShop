using KamalOnLineShop.Data;
using KamalOnLineShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KamalOnLineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.ApplicationUsers.ToList());
        }

         public async Task <IActionResult> Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    var isSaveRole = await _userManager.AddToRoleAsync(user, "User");
                    TempData["save"] = "User Created Succesufully";
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
           
            return View();
        }

        public IActionResult Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var user=_context.ApplicationUsers.FirstOrDefault(x=>x.Id==id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(ApplicationUser user)
        {
            var userupdated = _context.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);
            if(userupdated == null)
            {
                return NotFound();
            }
            userupdated.FirstName=user.FirstName;
            userupdated.LastName=user.LastName;

            var result = await _userManager.UpdateAsync(userupdated);
                if (result.Succeeded)
                {
                    TempData["edit"] = "User updated Succesufully";
                    return RedirectToAction(nameof(Index));

                }
            return View(userupdated);
            }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public IActionResult Lockout(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lockout(ApplicationUser user)
        {
            var userupdated = _context.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);
            if (userupdated == null)
            {
                return NotFound();
            }
            userupdated.LockoutEnd = DateTime.Now.AddYears(100);
            int rowEffected = _context.SaveChanges();
            if(rowEffected > 0)
            {
                TempData["lock"] = "User Lockedout Succesufully";
                return RedirectToAction(nameof(Index));
            }
            return View(userupdated);
        }

        public IActionResult Active(string id)
        {
            var user=_context.ApplicationUsers.FirstOrDefault(x=>x.Id==id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Active(ApplicationUser user)
        {
            var userupdated = _context.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);
            if (userupdated == null)
            {
                return NotFound();
            }
            userupdated.LockoutEnd = DateTime.Now.AddDays(-1);
            int rowEffected = _context.SaveChanges();
            if (rowEffected > 0)
            {
                TempData["save"] = "User has been Active Succesufully";
                return RedirectToAction(nameof(Index));
            }
            return View(userupdated);
        }

        public IActionResult Delete(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            var userupdated = _context.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);
            if (userupdated == null)
            {
                return NotFound();
            }
            _context.ApplicationUsers.Remove(userupdated);
            int rowEffected = _context.SaveChanges();
            if (rowEffected > 0)
            {
                TempData["lock"] = "User has been Deleted Succesufully";
                return RedirectToAction(nameof(Index));
            }
            return View(userupdated);
        }


    }
}

