using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.DomainLayer;
using Terminal3.ServiceLayer;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3WebAPI.Models;

namespace Terminal3WebAPI.Controllers
{
    /// <summary>
    /// API controller for all the registered user functunality
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegisteredUserController : ControllerBase
    {
        //Fields
        private readonly IECommerceSystem system;

        //Constructor
        public RegisteredUserController(IECommerceSystem system)
        {
            this.system = system;
        }

        #region End-Points
        /// <summary>
        /// Login to the system
        /// Template of valid JSON:
        /// {
        ///     "Email":"string",
        ///     "Password":"string"
        /// }
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public IActionResult Login([FromBody] CredentialsModel data)
        {
            Result<RegisteredUserService> result = system.Login(data.Email, data.Password);
            if (result.ExecStatus) { return Ok(result.Data.Id); }
            else { return BadRequest(result.Message); }

        }
        /// <summary>
        /// logout from the system
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <returns></returns>
        [Route("LogOut/{email}")]
        [HttpPost]
        public IActionResult LogOut(String email)
        {
            Result<Boolean> result = system.LogOut(email);
            if (result.ExecStatus) { return Created("api/GuestUserController", result.Message); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// Open new store in the system
        /// </summary>
        /// Template of valid JSON:
        /// {
        ///     "storeName":"string",
        ///     "userID":"string"
        /// }
        /// <param name="data"></param>
        [Route("OpenNewStore")]
        [HttpPost]
        public IActionResult OpenNewStore([FromBody] OpenNewStoreModel data)
        {
            Result<StoreService> result = system.OpenNewStore(data.storeName,data.userID);
            if (result.ExecStatus) { return Created("", result.Message); }
            else { return BadRequest(result.Message); }
        }
        /// <summary>
        /// Adding a new review on a product
        /// </summary>
        /// Template of valid JSON:
        /// {
        ///     "userID":"string",
        ///     "storeID":"string"
        ///     "productID":"string"
        ///     "review":"string"
        /// }
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("AddProductReview")]
        [HttpPost]
        public IActionResult AddProductReview([FromBody] ReviewModel data)
        {
            Result<ProductService> result = system.AddProductReview(data.userID,data.storeID,data.productID,data.review);
            if (result.ExecStatus) { return Created("", result.Message); }
            else { return BadRequest(result.Message); }
        }
        #endregion
    }
}
