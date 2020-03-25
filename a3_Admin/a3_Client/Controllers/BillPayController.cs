using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using a3_Client.Models;
using a3_Client.Utilities;
using Newtonsoft.Json;
using a3_Client.Attributes;

namespace a3_Client.Controllers
{
    [AuthorizeAdmin]
    public class BillPayController : Controller
    {
        // GET: customers/Index
        public async Task<IActionResult> Index()
        {
            var response = await Helper.InitializeClient().GetAsync("api/billPay");

            if (!response.IsSuccessStatusCode)
            {
                Helper.ClearAuth();
                return RedirectToAction("Check", "Home");
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var billPays = JsonConvert.DeserializeObject<List<BillPay>>(result);

            return View(billPays);
        }

        // GET: billPay/toggleBlock/1
        public async Task<IActionResult> ToggleBlock(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await Helper.InitializeClient().GetAsync($"api/billPay/toggleBlock/{id}");

            if (!response.IsSuccessStatusCode)
            {
                Helper.ClearAuth();
                return RedirectToAction("Check", "Home");
            }

            return RedirectToAction("Index");
        }
    }
}
