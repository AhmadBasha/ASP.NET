using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using a3_WebApi.Models;
using a3_WebApi.Models.DataManager;

namespace a3_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillPayController : Controller
    {
        private readonly BillPayManager _repo;

        public BillPayController(BillPayManager repo)
        {
            _repo = repo;
        }

        // GET: api/customers
        [HttpGet]
        public IEnumerable<BillPay> Get()
        {
            return _repo.GetAll();
        }

        // GET api/customers/1
        [HttpGet("{id}")]
        public BillPay Get(int id)
        {
            return _repo.Get(id);
        }

        [HttpGet("toggleBlock/{id}")]
        public long ToggleBlock(int id)
        {
            return _repo.ToggleBlock(id);
        }
    }
}
