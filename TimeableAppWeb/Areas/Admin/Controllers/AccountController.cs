using System.Threading.Tasks;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimeableAppWeb.Areas.Admin.ViewModels.Account;

namespace TimeableAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult ChangePassword()
        {
            var vm = new ChangePasswordViewModel
            {
                PasswordSuccessfullyChanged = false
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, vm.OldPassword, vm.Password);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(vm);
            }

            await _signInManager.RefreshSignInAsync(user);
            vm.PasswordSuccessfullyChanged = true;
            vm.Password = "";
            vm.ConfirmPassword = "";
            return View(vm);
        }

        public async Task<IActionResult> ChangeUserInformation()
        {
            var user = await _userManager.GetUserAsync(User);
            var vm = new UserInformationViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                InformationSaved = false
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserInformation(UserInformationViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.Email = vm.Email;
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            var result = await _userManager.UpdateAsync(user);
            if (result != IdentityResult.Success)
            {
                vm.InformationSaved = false;
                return View(vm);
            }
            vm.InformationSaved = true;
            return View(vm);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}