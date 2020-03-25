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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace a2_s3644668_s3643929.Controllers
{
    [AuthorizeCustomer]
    public class BillPayController : Controller
    {
        // getting customerid from session variable                   this is session var
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        private int SessionAccountNumber;

        private Customer myCustomer = null;

        private BaseAccount myAccount = null;

        private readonly LoginContext _context;

        public BillPayController(LoginContext context)
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
                    .Include(x => x.BillPays)
                        .ThenInclude(billPay => billPay.Payee)
                    .FirstOrDefaultAsync(x => x.AccountNumber == AcNum);
            }
            return myAccount;
        }


        public async Task<IActionResult> Index() => View(await GetMyCustomer());

       
        public async Task<IActionResult> Create(int id)
        {
            HttpContext.Session.SetInt32(nameof(SessionAccountNumber), id);
            foreach (var acc in (await GetMyCustomer()).Accounts)
            {
                if (acc.AccountNumber == id)
                {
                    myAccount = null;
                    await GetMyAccount(id);

                    var billPay = new BillPay { AccountNumber = id, Blocked = false };
                    myAccount.BillPays.Add(billPay);
                    return View(acc.BillPays.Last());
                }
            }

            return RedirectToAction("Index", "BillPay");
        }

        public IActionResult CreatePayee() => View();

        [HttpPost]
        public async Task<IActionResult> Delete(IFormCollection form)
        {
            int SessionEditID = (int) HttpContext.Session.GetInt32(nameof(SessionEditID));
            SessionAccountNumber = (int)HttpContext.Session.GetInt32(nameof(SessionAccountNumber));

            int.TryParse(form["BillPayID"], out int delId);

            if (delId == SessionEditID)
            {
                System.Diagnostics.Debug.WriteLine(" BP ID delete : " + delId);

                _context.Remove(await _context.BillPays.FindAsync(delId));
                await _context.SaveChangesAsync();

            }
            return View("DisplayBP", await GetMyAccount(SessionAccountNumber));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormCollection form)
        {
            int SessionEditID;
            int.TryParse(form["editID"], out int id);
            
            if (id != 0)
            {
                System.Diagnostics.Debug.WriteLine(" BP ID : " + id);
                SessionEditID = id;
                HttpContext.Session.SetInt32(nameof(SessionEditID), SessionEditID);
                
                return View(await _context.BillPays.FindAsync(id));
            }

            else
            {
                SessionEditID = (int) HttpContext.Session.GetInt32(nameof(SessionEditID));
                SessionAccountNumber = (int)HttpContext.Session.GetInt32(nameof(SessionAccountNumber));

                var billPay = new BillPay { };

                billPay.BillPayID = SessionEditID;
                billPay.AccountNumber = SessionAccountNumber;
                billPay.Amount = double.Parse(form["Amount"]);
                billPay.CustomerID = CustomerID;
                billPay.PayeeID = int.Parse(form["PayeeID"]);
                billPay.Period = (PeriodType)Enum.Parse(typeof(PeriodType), form["Period"]);
                billPay.ScheduleDate = DateTime.Parse(form["ScheduleDate"]);
                billPay.ModifyDate = DateTime.UtcNow;
                billPay.Count = 0;
                billPay.Blocked = bool.Parse(form["Blocked"]);

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(billPay);
                        await _context.SaveChangesAsync();

                        myAccount = null;
                        //acc.BillPays.Remove(acc.BillPays.Find(x => x.BillPayID == SessionEditID));
                        //acc.BillPays.Add(billPay);
                        //HttpContext.Session.Remove(nameof(SessionAccountNumber));
                        HttpContext.Session.Remove(nameof(SessionEditID));
                        //ActionResult action = new BillPayController(_context).DisplayBP(acc));
                        return View("DisplayBP", await GetMyAccount(SessionAccountNumber));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

            }
        }


        [HttpPost]
        public async Task<IActionResult> DisplayBP(IFormCollection form)
        {

            string strValue = form["SelectAcc"].ToString();

            int.TryParse(strValue, out int id);
            HttpContext.Session.SetInt32(nameof(SessionAccountNumber), id);

            foreach (var acc in (await GetMyCustomer()).Accounts)
            {
                if (acc.AccountNumber == id)
                {
                    return View(await GetMyAccount(id));
                }
            }

            return RedirectToAction("Index", "BillPay");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePayee([Bind("PayeeName,Address,City,State,PostCode,Phone")] Payee payee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    payee.Balance = 0.0;
                    _context.Update(payee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(payee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // adding rating
        public async Task<IActionResult> Create(IFormCollection form)
        {
            var billPay = new BillPay { };

            //int.TryParse(form["AccountNumber"].ToString(), out int acNum);


            SessionAccountNumber = (int) HttpContext.Session.GetInt32(nameof(SessionAccountNumber));
            int id = SessionAccountNumber;

            billPay.AccountNumber = SessionAccountNumber;
            billPay.Amount = double.Parse(form["Amount"]);
            billPay.CustomerID = CustomerID;
            billPay.PayeeID = int.Parse(form["PayeeID"]);
            billPay.Period = (PeriodType) Enum.Parse(typeof(PeriodType), form["Period"]);
            billPay.ScheduleDate = DateTime.Parse(form["ScheduleDate"]);
            billPay.ModifyDate = DateTime.UtcNow;
            billPay.Count = 0;
            billPay.Blocked = false;

            if (ModelState.IsValid)
            {
                try
                {

                    var payee = await _context.Payees.FindAsync(billPay.PayeeID);

                    if ( payee == null)
                    {
                        ModelState.AddModelError("PayeeID", "Selected payee doesn't exist. Chose from: ["+  string.Join(", ", _context.Payees.Select(x => x.PayeeID).ToList()) +"]");

                        return View(billPay);
                    }

                    myAccount = null;
                    await GetMyAccount(id);
                    if (myAccount.Balance < billPay.Amount )
                    {
                        ModelState.AddModelError("Amount", "You do not have sufficient Balance for this Amount!");

                        return View(billPay);
                    }

                        _context.Update(billPay);
                    await _context.SaveChangesAsync();
                    myAccount = null;
                    //HttpContext.Session.Remove(nameof(SessionAccountNumber));
                    return View("DisplayBP", await GetMyAccount(SessionAccountNumber));
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
