using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using a2_s3644668_s3643929.Data;
using a2_s3644668_s3643929.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a2_s3644668_s3643929.Attributes;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace a2_s3644668_s3643929.Controllers
{
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        // getting customerid from session variable                   this is session var
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        private Customer myCustomer = null;

        private BaseAccount myAccount = null;

        private readonly LoginContext _context;

        public CustomerController(LoginContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetMyCustomer()
        {
            if( myCustomer == null )
            {
                // Eager loading.
                myCustomer = await _context.Customers
                    .Include(x => x.Accounts)
                    .FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            }

            return myCustomer;
        }

        public async Task<BaseAccount> GetMyAccount(int AcNum)
        {
            if (myAccount == null || myAccount.AccountNumber != AcNum)
            {
                // Eager loading.
                myAccount = await _context.Accounts
                    .Include(x => x.Transactions)
                    .FirstOrDefaultAsync(x => x.AccountNumber == AcNum);
            }
            return myAccount;
        }

        public async Task<IActionResult> Index() => View(await GetMyCustomer());


        public async Task<IActionResult> Statement() => View(await GetMyCustomer());

        [HttpPost]
        public async Task<IActionResult> Display(IFormCollection form)
        {
            string strValue = form["SelectAcc"].ToString();

            int.TryParse(strValue, out int id);


            foreach (var acc in (await GetMyCustomer()).Accounts)
            {
                if (acc.AccountNumber == id)
                {
                    return View(await GetMyAccount(id));
                }
            }

            return RedirectToAction("Index", "Customer");
        }
    }
}
