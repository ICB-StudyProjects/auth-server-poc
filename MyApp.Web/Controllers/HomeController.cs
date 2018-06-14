namespace MyApp.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyApp.Web.Models;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public async Task Login()
        //{


        //    return RedirectToAction("Index");
        //}

        public async Task Logout()
        {
            // TODO: IdentityServer logs out but returns 
            // TODO: Does not remove the antiforgery token from cookies
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, "Cookies");
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext, "oidc");
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
