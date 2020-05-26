using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TimeableAppWeb.ViewModels;

namespace TimeableAppWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;


        public HomeController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index(bool noActiveScreen = false, bool passwordChanged = false, bool userActivated = false)
        {
            if (User != null)
            {
                if (_signInManager.IsSignedIn(User))
                {
                    return RedirectToAction("Index", "Home", new {Area = "Admin"});
                }
            }

            var inputModel = new HomeInputViewModel
            {
                NoActiveScreen = noActiveScreen,
                PasswordChanged = passwordChanged,
                UserActivated = userActivated,
                ShowUserNotActive = false
            };

            return View(inputModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(HomeInputViewModel input)
        {
            input.NoActiveScreen = false;
            input.PasswordChanged = false;
            input.UserActivated = false;
            input.ShowUserNotActive = false;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(input.Email);
                if (user == null || !user.EmailConfirmed)
                {
                    input.ShowUserNotActive = true;
                    return View(input);
                }
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }

                ModelState.AddModelError(string.Empty, Resources.Views.ClientHome.Index.InvalidLogin);
                input.Email = "";
                input.Password = "";
                return View(input);
            }

            // If we got this far, something failed, redisplay form
            return View(input);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // ================================== i18n ===================================
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                key: CookieRequestCultureProvider.DefaultCookieName,
                value: CookieRequestCultureProvider.MakeCookieValue(
                    requestCulture: new RequestCulture(culture: culture)),
                options: new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(years: 1)
                }
            );

            return LocalRedirect(localUrl: returnUrl);
        }
    }
}
