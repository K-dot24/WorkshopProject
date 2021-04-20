﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.ServiceLayer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Terminal3Controller : ControllerBase
    {
        public ECommerceSystem mySystem = new ECommerceSystem();

        // GET: api/<Terminal3Controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Terminal3Controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Terminal3Controller>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Terminal3Controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Terminal3Controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}