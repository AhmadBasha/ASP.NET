using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using a3_Client.Models;
using Microsoft.AspNetCore.Http;
using a3_Client.Utilities;

namespace a3_Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login", "Home");
        }

        // when i go login 
        [Route("secureAdminLoginXYZ")]
        public IActionResult Login()
        {
            if( Helper.HasAuth() )
            {
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                return View();
            }
        }

        // when i click on login 
        [HttpPost]
        [Route("secureAdminLoginXYZ")]
        public IActionResult Login(string UserID, string password)
        {
            //HttpContext.Session.SetString("uid", UserID);
            //HttpContext.Session.SetString("pwd", password);

            System.Diagnostics.Debug.WriteLine("HERE");

            Helper.SetUidPass(UserID, password);

            return RedirectToAction("Index", "Customer");
        }

        [Route("secureAdminLogoutXYZ")]
        public IActionResult Logout()
        {
            Helper.ClearAuth();

            return View("Login", "Home");
        }

        public IActionResult Check() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
