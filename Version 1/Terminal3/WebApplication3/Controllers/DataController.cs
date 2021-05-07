using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.ServiceLayer;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3WebAPI.Controllers
{
    /// <summary>
    /// This controller does not hold any functional requirements, it is only to transfer data to the view 
    /// </summary>
    /// 
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        //Fields
        private readonly IECommerceSystem system;

        //Constructor
        public DataController(IECommerceSystem system)
        {
            this.system = system;
        }

        [Route("GetAllStoresToDisplay")]
        [HttpGet]
        public IActionResult GetAllStoresToDisplay()
        {
            List<StoreService> result = system.GetAllStoresToDisplay();
            return Ok(result);
        }

        [Route("GetAllProductByStoreIDToDisplay/{storeID}")]
        [HttpGet]
        public IActionResult GetAllProductByStoreIDToDisplay(string storeID)
        {
            List<ProductService> result = system.GetAllProductByStoreIDToDisplay(storeID);
            return Ok(result);
        }
    }
}
