using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using a2_s3644668_s3643929.Attributes;
using a2_s3644668_s3643929.Data;
using a2_s3644668_s3643929.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleHashing;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace a2_s3644668_s3643929.Controllers
{
    [AuthorizeCustomer]
    public class ProfileController : Controller
    {
        // getting customerid from session variable                   this is session var
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        private Customer myCustomer = null;

        private readonly LoginContext _context;

        public ProfileController(LoginContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetMyCustomer()
        {
            if (myCustomer == null)
            {
                // Eager loading.
                myCustomer = await _context.Customers
                    .Include(x => x.Accounts)
                    .FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            }
            return myCustomer;
        }

        public async Task<Login> GetMyLogin()
        {
            await GetMyCustomer();

            var login = _context.Logins.Where(x => x.CustomerID == CustomerID).SingleOrDefault();
            return login;
        }

        public async Task<IActionResult> Index() => View(await GetMyCustomer());

        public async Task<IActionResult> Edit() => View(await GetMyCustomer());

        [HttpPost]
        [ValidateAntiForgeryToken]
        // adding rating
        public async Task<IActionResult> Edit([Bind("CustomerName,TFN,Address,City,State,PostCode,Phone")] Customer customer)
        {
            customer.CustomerID = this.CustomerID;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }


        public async Task<IActionResult> Password() => View(await GetMyLogin());

        [HttpPost]
        [ValidateAntiForgeryToken]
        // adding rating
        public async Task<IActionResult> Password(IFormCollection form)
        {
            var login = await GetMyLogin();

            //login.CustomerID = this.CustomerID;

            string oldPass = form["Password"];
            string newPass = form["newPass"];

            try
            {
                login.ChangePassword(oldPass, newPass);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Failed", e.Message);
                return View(login);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(login);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(login);
        }
    }
}
