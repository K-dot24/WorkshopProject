using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Terminal3.ServiceLayer;

namespace Terminal3WebAPI.Controllers
{
    public class Terminal3Controller : ApiController
    {
        public ECommerceSystem system = new ECommerceSystem();

        // GET: api/Terminal3
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Terminal3/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Terminal3
        public void Post([FromBody] string value)
        {
        }

        /*  // PUT: api/Terminal3/5
          public void Put(int id, [FromBody]string value)
          {
          }*/

        // DELETE: api/Terminal3/5
        public void Delete(int id)
        {
        }
    }
}
