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
    public class AccountController : Controller
    {
        // getting customerid from session variable                   this is session var
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        private Customer myCustomer = null;

        private BaseAccount myAccount = null;

        private readonly LoginContext _context;

        public AccountController(LoginContext context)
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

        public async Task<IActionResult> Deposit() => View(await GetMyCustomer());

        public async Task<IActionResult> Withdraw() => View(await GetMyCustomer());

        public async Task<IActionResult> Transfer() => View(await GetMyCustomer());

        [HttpPost]
        public async Task<IActionResult> Deposit(IFormCollection form)
        {
            double.TryParse(form["Amount"], out double amount);
            string strValue = form["SelectAcc"].ToString();
            int.TryParse(strValue, out int id);

            BaseAccount depositAcc = await GetMyAccount(id);

            try
            {
                depositAcc.Deposit(amount, form["Comment"]);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(amount), e.Message);
                ViewBag.Amount = amount;
                return View(await GetMyCustomer());
            }

            await _context.SaveChangesAsync();

            //return Receipt(CustomerID, form["Comment"]);

            return View("Receipt", (
                depositAcc.Transactions.Last().TransactionType.ToString(),
                depositAcc.Transactions.Last().TransactionID,
                depositAcc.Transactions.Last().ModifyDate,
                CustomerID,
                id,
                depositAcc.Transactions.Last().Amount,
                depositAcc.Transactions.Last().Comment,
                -1
                ));
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(IFormCollection form)
        {

            double.TryParse(form["Amount"], out double amount);

            string strValue = form["SelectAcc"].ToString();
            int.TryParse(strValue, out int id);

            // no need to check the account that is being pulled because it is a validated post.
            BaseAccount withdrawAcc = await GetMyAccount(id); ;

            try
            {
                withdrawAcc.Withdraw(amount, form["Comment"]);
            }
            catch( Exception e)
            {
                ModelState.AddModelError(nameof(amount), e.Message);
                ViewBag.Amount = amount;
                return View(await GetMyCustomer());
            }

            await _context.SaveChangesAsync();

            //return Receipt(CustomerID, form["Comment"]);

            return View("Receipt", (
                withdrawAcc.Transactions.Last().TransactionType.ToString(),
                withdrawAcc.Transactions.Last().TransactionID,
                withdrawAcc.Transactions.Last().ModifyDate,
                CustomerID,
                id,
                withdrawAcc.Transactions.Last().Amount,
                withdrawAcc.Transactions.Last().Comment,
                -1
                ));
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(IFormCollection form)
        {

            double.TryParse(form["Amount"], out double amount);

            string strValue = form["SelectAcc"].ToString();
            int.TryParse(strValue, out int id);

            string toAccStr = form["ToAccount"].ToString();
            int.TryParse(toAccStr, out int toAccNum);

            // no need to check the account that is being pulled because it is a validated post.
            BaseAccount withdrawAcc = await GetMyAccount(id); ;

            BaseAccount depositAcc = await _context.Accounts
                    .Include(x => x.Transactions)
                    .FirstOrDefaultAsync(x => x.AccountNumber == toAccNum);

            try
            {
                withdrawAcc.Transfer(amount, depositAcc, form["Comment"]);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(nameof(amount), e.Message);
                ViewBag.Amount = amount;
                return View(await GetMyCustomer());
            }

            await _context.SaveChangesAsync();

            //return Receipt(CustomerID, form["Comment"]);

            return View("Receipt", (
                withdrawAcc.Transactions.Last().TransactionType.ToString(),
                withdrawAcc.Transactions.Last().TransactionID,
                withdrawAcc.Transactions.Last().ModifyDate,
                CustomerID,
                id,
                withdrawAcc.Transactions.Last().Amount,
                withdrawAcc.Transactions.Last().Comment,
                toAccNum
                ));
        }
    }
}
