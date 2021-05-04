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
    /// API controller for all the guest user functunality
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GuestUserController : ControllerBase
    {
        //Fields
        private readonly IECommerceSystem system;

        //Constructor
        public GuestUserController(IECommerceSystem system)
        {
            this.system = system;
        }

        #region End-Points
        /// <summary>
        /// Get welcome page of the system
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EnterSystem() {
            return Ok("Welcome to Terminal3 system");
        }

        /// <summary>
        /// Signal the system that the user is exited
        /// </summary>
        /// <param name="userID"> string of the userID - need to be in the body of the request</param>
        /// <returns></returns>
        //[Route("ExitSystem")]
        [Route("ExitSystem/{userID}")]
        [HttpGet]
        public IActionResult ExitSystem(string userID) {
            system.ExitSystem(userID);
            return Ok();
        }

        /// <summary>
        /// Register new user to the system
        /// </summary>
        /// Template of valid JSON:
        /// {
        ///     "Email":"string",
        ///     "Password":"string"
        /// }
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("Register")]
        [HttpPost]
        public IActionResult Register([FromBody] User user) 
        {
            Result<RegisteredUserService> result =  system.Register(user.Email, user.Password);
            if (result.ExecStatus) 
            {
                return Created("api/registeruser/Login", result.Data.Id);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        /// <summary>
        /// search store by attributes
        /// Template of valid JSON
        /// {
        ///     "Name":"string",
        ///     "rating":double
        /// }
        /// NOTE: fields are optionals
        /// </summary>
        /// <param name="storeDetails"></param>
        [Route("SearchStore")]
        [HttpGet]
        public IActionResult SearchStore([FromBody] IDictionary<string, object> storeDetails)
        {
            Result<List<StoreService>> result = system.SearchStore(storeDetails);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result); }
            //return system.SearchStore(storeDetails);
        }

        /// <summary>
        /// Search product by attributes
        /// Template of valid JSON:
        /// {
        ///     "Name":"string",
        ///     "Category":"string",
        ///     "Lowprice":double,
        ///     "Highprice":double,
        ///     "Productrating":double,
        ///     "Storerating":double,
        ///     "Keywords":["string","string"]
        /// }
        /// NOTE: fields are optionals
        /// </summary>
        /// <param name="productDetails"></param>
        /// <returns></returns>
        [Route("SearchProduct")]
        [HttpGet]
        public IActionResult SearchProduct([FromBody] IDictionary<string, object> productDetails)
        {
            Result < List < ProductService >> result = system.SearchProduct(productDetails);
            if (result.ExecStatus) { return Ok(result); } 
            else { return BadRequest(result); }
        }

        /// <summary>
        /// Adding product to user's cart
        /// Template of valid JSON:
        /// {
        ///     "userID":"string",
        ///     "ProductID":"string",
        ///     "ProductQuantity":int,
        ///     "StoreID":"string"
        /// }
        /// </summary>
        /// <param name="productToCart"></param>
        [Route("AddProductToCart")]
        [HttpPost]
        public IActionResult AddProductToCart([FromBody] ProductToCart productToCart)
        {
            Result < Boolean > result = system.AddProductToCart(productToCart.userID, productToCart.ProductID, productToCart.ProductQuantity, productToCart.StoreID);
            if (result.ExecStatus) { return Created($"GetUserShoppingCart/{productToCart.userID}",null); }
            else { return BadRequest(result.Message); } 
        }

        /// <summary>
        /// Return the user's shopping cart
        /// </summary>
        /// <param name="userID">string of the user's ID</param>
        //[Route("GetUserShoppingCart")]
        [Route("GetUserShoppingCart/{userID}")]
        [HttpGet]
        public IActionResult GetUserShoppingCart(String userID)
        {
            Result<ShoppingCartService> result = system.GetUserShoppingCart(userID);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result); }
        }

        /// <summary>
        /// Updates the user's shopping cart with the current quantity of a product
        /// Template of valid JSON:
        /// {
        ///     "userID":"string",
        ///     "shoppingBagID":"string",
        ///     "productID":"string",
        ///     "quantity":int
        /// }
        /// </summary>
        /// <param name="details">identifier of the user,shop,product and its new quantity</param>
        [Route("UpdateShoppingCart")]
        [HttpPut]
        public IActionResult UpdateShoppingCart([FromBody] UpdateShoppingCartModel details)
        {
            Result<Boolean> result = system.UpdateShoppingCart(details.userID, details.shoppingBagID, details.productID, details.quantity);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result); }
        }

        /// <summary>
        /// Preform purchase operation on the user shopping cart
        /// Template of valid JSON:
        /// {
        ///     "userID":"string",
        ///     "paymentDetails":{NOT IMPLEMENTED
        ///                         }
        ///     "deliveryDetails":{NOT IMPLEMENTED
        ///                         }
        /// }
        /// </summary>
        /// <param name="purchaseDetails">userID, paymant and delivery details</param>
        [Route("Purchase")]
        [HttpPut]
        public IActionResult Purchase([FromBody] PurchaseModel purchaseDetails)
        {
            Result<ShoppingCartService> result = system.Purchase(purchaseDetails.userID, purchaseDetails.paymentDetails, purchaseDetails.deliveryDetails);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result); }
        }

        /// <summary>
        /// Retrive the user purchase history
        /// </summary>
        /// <param name="userID">string of the userID - need to be in the body of the request</param>
        //[Route("GetUserPurchaseHistory")]
        [Route("GetUserPurchaseHistory/{userID}")]
        [HttpGet]
        public IActionResult GetUserPurchaseHistory(String userID)
        {
            Result<HistoryService> result = system.GetUserPurchaseHistory(userID);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result); }
        }

        /// <summary>
        /// Return the total amount of the user's shopping cart
        /// </summary>
        /// <param name="userID"></param>
        [Route("GetTotalShoppingCartPrice/{userID}")]
        [HttpGet]
        public IActionResult GetTotalShoppingCartPrice( String userID)
        {
            Result<double> result = system.GetTotalShoppingCartPrice(userID);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result); }
        }

        /// <summary>
        /// Returns all the reviews on a specific product
        /// </summary>
        /// <param name="storeID">identifier of the store</param>
        /// <param name="productID">identifier of the product</param>
        [Route("GetProductReview/{storeID}/{productID}")]
        [HttpGet]
        public IActionResult GetProductReview(string storeID, string productID)
        {
            Result<List<Tuple<String, String>>> result = system.GetProductReview(storeID,productID);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result); }
        }
        #endregion
    }
}
