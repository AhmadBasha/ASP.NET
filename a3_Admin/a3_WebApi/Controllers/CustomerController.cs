using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using a3_WebApi.Models;
using a3_WebApi.Models.DataManager;
using System.Linq;

namespace a3_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly CustomerManager _repo;

        public CustomerController(CustomerManager repo)
        {
            _repo = repo;
        }

        // GET: api/customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return _repo.GetAll();
        }

        // GET api/customers/1
        [HttpGet("{id}")]
        public Customer Get(int id)
        {
            return _repo.Get(id);
        }

        // POST api/customers
        [HttpPost]
        public void Post([FromBody] Customer customer)
        {
            _repo.Add(customer);
        }

        // PUT api/customers
        [HttpPut]
        public void Put([FromBody] Customer customer)
        {
            _repo.Update(customer.CustomerID, customer);
        }

        // DELETE api/customers/1
        [HttpDelete("{id}")]
        public long Delete(int id)
        {
            return _repo.Delete(id);
        }

        [HttpGet("lockout/{id}")]
        public long Lockout(int id)
        {
            return _repo.Lockout(id);
        }

        [HttpGet("getNested/{CustId}/{AccId}/{fromLong:long}/{toLong:long}/{Keywords}")]
        [HttpGet("getNested/{CustId}/{AccId}/{fromLong:long}/{toLong:long}")]
        [HttpGet("getNested/{CustId}/{AccId}")]
        [HttpGet("getNested/{CustId}")]
        [HttpGet("getNested")]
        public IEnumerable<Customer> GetNested(int? CustId, int? AccId, long? fromLong, long? toLong, string Keywords)
        {
            List<Customer> custs = _repo.GetAll().ToList();

            if (CustId.HasValue)
            {
                var replaceCust = custs.FirstOrDefault(x => x.CustomerID == CustId);
                if (replaceCust != null)
                {
                    replaceCust = _repo.Get((int)CustId);
                }

                if (AccId.HasValue)
                {
                    var replaceAcc = custs.FirstOrDefault(x => x.CustomerID == CustId).Accounts.FirstOrDefault(y => y.AccountNumber == AccId);

                    if (replaceAcc != null)
                    {
                        if (fromLong.HasValue)
                        {
                            replaceAcc = _repo.FilterTransactions((int)AccId, (long)fromLong, (long)toLong, Keywords);
                        }
                        else
                        {
                            replaceAcc = _repo.GetAcc((int)AccId);
                        }
                    }
                }
            }
            return custs;
        }
    }
}
