using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleHashing;
using a2_s3644668_s3643929.Data;
using a2_s3644668_s3643929.Models;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace a2_s3644668_s3643929.Controllers
{
   
    public class LoginController : Controller
    {
        // extracting 
        private readonly LoginContext _context;
        // GET: /<controller>/

        public LoginController(LoginContext context)
        {
            _context = context;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}

        // when the user enter the page 
        public IActionResult Logins()
        {
            if (HttpContext.Session.GetInt32(nameof(Customer.CustomerID)) == null)
            {
                return View();
            }

            return RedirectToAction("Index", "Customer");
        }


        // when the user put values for loggin 
        [HttpPost]
        public async Task<IActionResult> Logins(string UserID, string password)
        {

            int.TryParse(UserID, out int InputID);
            var login = await _context.Logins.FindAsync(InputID);

            if (login == null)
            {
                //A3
               ModelState.AddModelError("LoginFailed", "INVALID USER OR PASSWORD");

                //throw new Exception("LOL");

                return View();
            }

            try
            {
                login.AttemptLogin(password);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("LoginFailed", e.Message);

                await _context.SaveChangesAsync();
                return View();
            }

            // Login customer.
            await _context.SaveChangesAsync();

            login.Customer = await _context.Customers.FindAsync(login.CustomerID);

            //System.Diagnostics.Debug.WriteLine("2 Login object = " + login);
            //System.Diagnostics.Debug.WriteLine("2 Customer object = " + login.Customer);



            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.CustomerName), login.Customer.CustomerName);

            //System.Diagnostics.Debug.WriteLine(UserID + " : " + login.Customer.CustomerName);

            return RedirectToAction("Index", "Customer");

        }

        [Route("LogoutNow")]
        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }


    }
}
