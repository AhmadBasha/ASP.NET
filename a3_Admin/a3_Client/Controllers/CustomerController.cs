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
    public class CustomerController : Controller
    {
        // GET: customers/Index
        public async Task<IActionResult> Index()
        {
            var response = await Helper.InitializeClient().GetAsync("api/customer");

            if (!response.IsSuccessStatusCode)
            {
                Helper.ClearAuth();
                return RedirectToAction("Check", "Home");
            }

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            // Deserializing the response recieved from web api and storing into a list.
            var customers = JsonConvert.DeserializeObject<List<Customer>>(result);

            return View(customers);
        }

        // GET: customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = Helper.InitializeClient().PostAsync("api/customer", content).Result;

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: customers/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await Helper.InitializeClient().GetAsync($"api/customer/{id}");

            if (!response.IsSuccessStatusCode)
            {
                Helper.ClearAuth();
                return RedirectToAction("Check", "Home");
            }

            var result = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<Customer>(result);

            return View(customer);
        }

        // POST: customers/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Customer customer)
        {
            if (id != customer.CustomerID)
                return NotFound();

            if (ModelState.IsValid)
            {
                var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = Helper.InitializeClient().PutAsync("api/customer", content).Result;

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: customers/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await Helper.InitializeClient().GetAsync($"api/customer/{id}");
            // having wrong auth
            if (!response.IsSuccessStatusCode)
            {
                Helper.ClearAuth();
                return RedirectToAction("Check", "Home");
            }

            var result = response.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<Customer>(result);

            return View(customer);
        }

        // POST: customers/Delete/1
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var response = Helper.InitializeClient().DeleteAsync($"api/customer/{id}").Result;

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return NotFound();
        }

        public ActionResult LockoutSuccess(Customer customer) => View(customer);

        // GET: customers/Lockout/1
        public async Task<IActionResult> Lockout(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await Helper.InitializeClient().GetAsync($"api/customer/lockout/{id}");

            if (!response.IsSuccessStatusCode)
            {
                Helper.ClearAuth();
                return RedirectToAction("Check", "Home");
            }

            var retId = response.Content.ReadAsStringAsync().Result;



            // This will retrieve the customer that just got locked out.
            var response2 = await Helper.InitializeClient().GetAsync($"api/customer/{retId}");

            if (!response2.IsSuccessStatusCode)
            {
                Helper.ClearAuth();
                return RedirectToAction("Check", "Home");
            }

            var result = response2.Content.ReadAsStringAsync().Result;
            var customer = JsonConvert.DeserializeObject<Customer>(result);

            return View("LockoutSuccess", customer);
        }
    }
}
