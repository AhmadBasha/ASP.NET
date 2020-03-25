using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using a3_WebApi.Models;
using a3_WebApi.Models.DataManager;

namespace a3_WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly AccountManager _repo;

        public AccountController(AccountManager repo)
        {
            _repo = repo;
        }

        // GET: api/customers
        [HttpGet]
        public IEnumerable<BaseAccount> Get()
        {
            return _repo.GetAll();
        }

        // GET api/customers/1
        [HttpGet("{id}")]
        public BaseAccount Get(int id)
        {
            return _repo.Get(id);
        }
    }
}
