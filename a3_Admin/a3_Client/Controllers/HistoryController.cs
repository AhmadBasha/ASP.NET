using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using a3_Client.Models;
using a3_Client.Utilities;
using Newtonsoft.Json;
using a3_Client.Attributes;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace a3_Client.Controllers
{
    [AuthorizeAdmin]
    public class HistoryController : Controller
    {

        internal List<Customer> customers = null;

        // getting all the data in a correct format 
        public async Task<bool> LoadCustomers(int? CustId, int? AccId, DateTime? fromDate, DateTime? toDate, string keywords)
        {
            string getString = "api/customer/getNested";

            if(CustId.HasValue)
            {
                getString += "/" + CustId;

                if(AccId.HasValue)
                {
                    getString += "/" + AccId;
                }
            }

            if(fromDate != null)
            {
                getString += "/" + fromDate.Value.ToBinary() + "/" + toDate.Value.ToBinary();

                if(!string.IsNullOrEmpty(keywords))
                {
                    getString += "/" + keywords;
                }
            }



            System.Diagnostics.Debug.WriteLine("GET STR = " + getString);

            var response = await Helper.InitializeClient().GetAsync(getString);

            if (!response.IsSuccessStatusCode)
            {
                Helper.ClearAuth();
                return false; 
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            customers = JsonConvert.DeserializeObject<List<Customer>>(result);

            if( CustId.HasValue && AccId.HasValue)
            {
                List<double> numbers = new List<double>();
                List<string> dates = new List<string>();

                // Even adding new data pt 1
                List<double> cumulative = new List<double>();
                List<double> numbers2 = new List<double>();
              

                double sum = 0;

                foreach (var item in customers
                    .Where(x => x.CustomerID == CustId)
                    .FirstOrDefault().Accounts.Where(y => y.AccountNumber == AccId)
                    .FirstOrDefault().Transactions)
                {
                    double num = item.TransactionType == TransactionType.Deposit ? item.Amount : 0 * item.Amount;
                    numbers.Add(num);

                    //Evan
                    double num2 = item.TransactionType == TransactionType.Withdraw ? item.Amount : 0 * item.Amount;
                    numbers2.Add(num2);

            


                    dates.Add(item.ModifyDate.ToShortDateString());


                    // Evan adding amount to cumulative
                    sum += num;
                    cumulative.Add(sum);

                }

                ViewBag.numbers = numbers;
                ViewBag.numbers2 = numbers2;
 
                ViewBag.dates = dates;
                ViewBag.cumulative = cumulative;
            }


                return true;
        }

        public async Task<IActionResult> Index()
        {

            if( await LoadCustomers(null, null, null, null, null) )
            {
                return View(customers);
            }
            else
            {
                return RedirectToAction("Check", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChoseCustomer(IFormCollection form)
        {
            string strValue = form["SelectCust"].ToString();

            int.TryParse(strValue, out int custId);

            if( custId == 0 )
            {
                return RedirectToAction("Index");
            }

            if (await LoadCustomers(custId, null, null, null, null))
            {
                ViewBag.SelectCust = custId;
                return View("Index", customers);
            }
            else
            {
                return RedirectToAction("Check", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChoseAccount(IFormCollection form)
        {
            string AstrValue = form["SelectAcc"].ToString();

            int.TryParse(AstrValue, out int accId);

            string CstrValue = form["SelectCust"].ToString();

            int.TryParse(CstrValue, out int custId);

            if (custId == 0 || accId == 0)
            {
                return RedirectToAction("Index");
            }

            if (await LoadCustomers(custId, accId, null, null, null))
            {
                ViewBag.SelectCust = custId;
                ViewBag.SelectAcc = accId;
                return View("Index",customers);
            }
            else
            {
                return RedirectToAction("Check", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Filter(IFormCollection form)
        {
            System.Diagnostics.Debug.WriteLine("2 Login object = " + string.Join(", ", form.Keys));
            System.Diagnostics.Debug.WriteLine("2 Login object = " + form["SelectAcc"]);

            string AstrValue = form["SelectAcc"].ToString();

            int.TryParse(AstrValue, out int accId);

            string CstrValue = form["SelectCust"].ToString();

            int.TryParse(CstrValue, out int custId);


            DateTime fromDate = DateTime.Parse(form["FromTime"]);
            DateTime toDate = DateTime.Parse(form["ToTime"]);

            string Keywords = form["Keywords"];


            if (custId == 0 || accId == 0)
            {
                return RedirectToAction("Index");
            }

            if (await LoadCustomers(custId, accId, fromDate, toDate, Keywords))
            {
                ViewBag.SelectCust = custId;
                ViewBag.SelectAcc = accId;
                return View("Index", customers);
            }
            else
            {
                return RedirectToAction("Check", "Home");
            }
        }
    }
}
