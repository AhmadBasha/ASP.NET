using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using a2_s3644668_s3643929.Models;
using a2_s3644668_s3643929.Data;
using Microsoft.AspNetCore.Diagnostics;

namespace a2_s3644668_s3643929.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LoginContext _context;

        public HomeController(ILogger<HomeController> logger, LoginContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //new LoginController(_context).Logins();

            return RedirectToAction("Logins", "Login");
        }

        // A3
        [Route("/Home/Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            //var statusCodeResult =
            //        HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            ErrorViewModel EM; 

            switch (statusCode)
            {
                case 404:

                    EM = new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        //ErrorPath = statusCodeResult.OriginalPath,
                        ErrorMessage = "Sorry, the resource you requested could not be found",
                        StatusCode = statusCode,
                        //QuerryString = statusCodeResult.OriginalQueryString
                    };
                    break;

                case 405:
                    EM = new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        //ErrorPath = statusCodeResult.OriginalPath,
                        ErrorMessage = "You are not supposed to go to that link!",
                        StatusCode = statusCode,
                        //QuerryString = statusCodeResult.OriginalQueryString
                    };
                    break;

                default:
                    EM = new ErrorViewModel
                    {
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                        //ErrorPath = statusCodeResult.OriginalPath,
                        ErrorMessage = "Invalid Operation",
                        StatusCode = statusCode,
                        //QuerryString = statusCodeResult.OriginalQueryString
                    };
                    break;
            }

            //return View("NotFound");

            return View("Error", EM);
        }

        //A3
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Retrieve the exception Details
            var exceptionHandlerPathFeature =
                    HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            //ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            //ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ExceptionPath = exceptionHandlerPathFeature.Path,
                ExceptionMessage = exceptionHandlerPathFeature.Error.Message,
                StackTrace = exceptionHandlerPathFeature.Error.StackTrace
            });
        }
    }
}
