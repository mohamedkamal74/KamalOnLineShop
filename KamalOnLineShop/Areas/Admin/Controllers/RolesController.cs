using KamalOnLineShop.Areas.ViewModels;
using KamalOnLineShop.Data;
using KamalOnLineShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KamalOnLineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RolesController(RoleManager<IdentityRole>roleManager,
            UserManager<IdentityUser>userManager,ApplicationDbContext context)
        {
            _roleManager = roleManager;
           _userManager = userManager;
            _context = context;
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
            ViewBag.UserId = _context.ApplicationUsers.Where(x=>x.LockoutEnd<DateTime.Now||x.LockoutEnd==null).ToList();
            ViewBag.RoleId = _roleManager.Roles.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(RoleUserVM roleuser)
        {
            var user=_context.ApplicationUsers.FirstOrDefault(x=>x.Id==roleuser.UserId);
            var checkisroleassign =await _userManager.IsInRoleAsync(user, roleuser.RoleId);
            if (checkisroleassign)
            {
                ViewBag.msg = "this user already assigned in this role";
                ViewBag.UserId = _context.ApplicationUsers.Where(x => x.LockoutEnd < DateTime.Now || x.LockoutEnd == null).ToList();
                ViewBag.RoleId = _roleManager.Roles.ToList();
                return View();

            }
            var role= await _userManager.AddToRoleAsync(user, roleuser.RoleId);
            if (role.Succeeded)
            {
                TempData["save"] = "User Role Assigned Succesufully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult AssignUserRole()
        {
            var result = from ur in _context.UserRoles
                         join rs in _context.Roles on ur.RoleId equals rs.Id
                         join us in _context.ApplicationUsers on ur.UserId equals us.Id
                         select new UserRoleMapping()
                         {
                             UserId = ur.UserId,
                             RoleId = ur.RoleId,
                             UserName = us.UserName,
                             RoleName = rs.Name
                         };
            ViewBag.userrole = result;
            return View();
        }
    }
}
