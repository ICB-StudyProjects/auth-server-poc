namespace MyApp.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using IdentityModel.Client;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;
    using Model;
    using MyApp.Web.Models;
    using Newtonsoft.Json;

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

        [Authorize]
        public async Task Logout()
        {
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

        [Authorize]
        public async Task<IActionResult> Values()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var valuesRes = await (await client.GetAsync("http://localhost:52934/api/values")).Content.ReadAsStringAsync();

                var values = JsonConvert.DeserializeObject<string[]>(valuesRes).Select(val => new Value(val));

                return View(values);
            }
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
