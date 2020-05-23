using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using TimeableAppWeb.ViewModels;

namespace TimeableAppWeb.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        // GET: Subjects
        public IActionResult ForgotPassword()
        {
            var vm = new ForgotPasswordViewModel
            {
                LinkSent = false
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    model.LinkSent = true;
                    return View(model);
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { code },
                    Request.Scheme);

                var htmlMessageText = "<h4>Password reset request</h4>" +
                                      "<p>You have requested for password reset in Timeable app. " +
                                      $"Password reset can be done by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking this link</a>.</p>";

                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Timeable: reset password",
                    htmlMessageText);
                
                model.LinkSent = true;
                return View(model);
            }

            return View(model);
        }

        public IActionResult ResetPassword(string? code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }

            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                Code = code
            };
            return View(resetPasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("Index", "Home", new {passwordChanged = true});
            }

            var result = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code)), model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { passwordChanged = true });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        public IActionResult ActivateAccountAndResetPassword(string? accountCode = null, string? passwordCode = null)
        {
            if (accountCode == null || passwordCode == null)
            {
                return BadRequest("A codes must be supplied for account activation and password reset.");
            }

            var activateAccountAndResetPasswordViewModel = new ActivateAccountAndResetPasswordViewModel
            {
                AccountActivationCode = accountCode,
                PasswordResetCode = passwordCode
            };
            return View(activateAccountAndResetPasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateAccountAndResetPassword(ActivateAccountAndResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("Index", "Home", new { userActivated = true });
            }

            var confirmEmail = await _userManager.ConfirmEmailAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(vm.AccountActivationCode)));
            if (!confirmEmail.Succeeded)
            {
                foreach (var error in confirmEmail.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(vm.PasswordResetCode)), vm.Password);
            if (result.Succeeded)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }

                return RedirectToAction("Index", "Home", new { userActivated = true });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
    }
}
