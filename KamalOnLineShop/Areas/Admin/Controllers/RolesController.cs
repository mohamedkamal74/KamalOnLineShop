using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace KamalOnLineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityRole> _userManager;

        public RolesController(RoleManager<IdentityRole>roleManager,
            UserManager<IdentityRole>userManager)
        {
            _roleManager = roleManager;
           _userManager = userManager;
        }
        public IActionResult Index()
        {
            ViewBag.roles = _roleManager.Roles.ToList();

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(string name)
        {
            IdentityRole role=new IdentityRole();
            role.Name = name;
            var existname=await _roleManager.RoleExistsAsync(role.Name);
            if(existname)
            {
                ViewBag.existName = "this Role ia already exist";
                ViewBag.name=name;
                return View();
            }
            var result=await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["save"] = "role Created Succesufully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task< IActionResult> Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var role= await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                return NotFound();
            }
            ViewBag.id=role.Id;
            ViewBag.name=role.Name;
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,string name)
        {

            var role =await _roleManager.FindByIdAsync( id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;
            var existname = await _roleManager.RoleExistsAsync(role.Name);
            if (existname)
            {
                ViewBag.existName = "this Role ia already exist";
                return View();
            }
            ViewBag.name=name;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["edit"] = "role Updated Succesufully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
           
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["lock"] = "role Deleted Succesufully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Assign()
        {
            ViewBag.userId = _userManager.Users.ToList();
            ViewBag.roleId = _roleManager.Roles.ToList();
            return View();
        }
    }
}
