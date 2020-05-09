using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TimeableAppWeb.ViewModels;

namespace TimeableAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "ScreenSettings");
        }

        public IActionResult Privacy()
        {
            return View();
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
