using Bootcamp.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Bootcamp.PresentationLayer.Areas.Admin.Models;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userViewModels = new List<UserWithRolesViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserWithRolesViewModel
                {
                    Id = user.Id,
                    NameSurname = user.NameSurname,
                    Email = user.Email,
                    Gender = user.Gender,
                    Roles = roles.ToList()
                });
            }
            return View(userViewModels);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles;
            return View(user);
        }

        public IActionResult Create()
        {
            ViewBag.AllRoles = _roleManager.Roles.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AllRoles = _roleManager.Roles.ToList();
                return View(model);
            }
            var user = new ApplicationUser
            {
                NameSurname = model.NameSurname,
                Email = model.Email,
                UserName = model.Email,
                Gender = model.Gender
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (model.SelectedRoles != null && model.SelectedRoles.Count > 0)
                {
                    await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                }
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            ViewBag.AllRoles = _roleManager.Roles.ToList();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            ViewBag.AllRoles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new UserEditViewModel
            {
                Id = user.Id,
                NameSurname = user.NameSurname,
                Email = user.Email,
                Gender = user.Gender,
                GeminiApiKey = user.GeminiApiKey,
                SelectedRoles = userRoles.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            var existingUser = await _userManager.FindByIdAsync(model.Id.ToString());
            if (existingUser == null) return NotFound();
            if (!ModelState.IsValid)
            {
                ViewBag.AllRoles = _roleManager.Roles.ToList();
                model.SelectedRoles = (await _userManager.GetRolesAsync(existingUser)).ToList();
                return View(model);
            }
            existingUser.NameSurname = model.NameSurname;
            existingUser.Email = model.Email;
            existingUser.UserName = model.Email;
            existingUser.Gender = model.Gender;
            existingUser.GeminiApiKey = model.GeminiApiKey;
            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(existingUser);
                var rolesToAdd = model.SelectedRoles?.Except(userRoles) ?? new List<string>();
                var rolesToRemove = userRoles.Except(model.SelectedRoles ?? new List<string>());
                await _userManager.AddToRolesAsync(existingUser, rolesToAdd);
                await _userManager.RemoveFromRolesAsync(existingUser, rolesToRemove);
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            ViewBag.AllRoles = _roleManager.Roles.ToList();
            model.SelectedRoles = (await _userManager.GetRolesAsync(existingUser)).ToList();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }
    }
}