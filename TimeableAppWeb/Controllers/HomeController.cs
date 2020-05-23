using System;
using System.ComponentModel.DataAnnotations;
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


        [BindProperty] public InputModel Input { get; set; } = default!;

        public class InputModel
        {
            public bool NoActiveScreen { get; set; } 
            public bool PasswordChanged { get; set; } 
            public bool UserActivated { get; set; }
            public bool ShowUserNotActive { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; } = default!;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = default!;
        }

        public IActionResult Index(bool noActiveScreen = false, bool passwordChanged = false, bool userActivated = false)
        {
            var inputModel = new InputModel
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
        public async Task<IActionResult> Index(InputModel input)
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

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
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
