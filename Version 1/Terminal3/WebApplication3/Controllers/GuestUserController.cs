using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.DomainLayer;
using Terminal3.ServiceLayer;
using Terminal3.ServiceLayer.ServiceObjects;
using Terminal3WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Terminal3WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestUserController : ControllerBase
    {
        private readonly IECommerceSystem mySystem;

        public GuestUserController(IECommerceSystem system)
        {
            this.mySystem = system;
        }




        /// <summary>
        /// Get welcome page of the system
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string EnterSystem() { return "Welcome to Terminal3 system"; }

        /// <summary>
        /// Signal the system that the user is exited
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route("ExitSystem/{userID}")]
        [HttpGet]
        public void ExitSystem(string userID) { mySystem.ExitSystem(userID); }

        /// <summary>
        /// Register new user to the system
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("Register")]
        [HttpPost]
        public IActionResult Register([FromBody] User user) 
        {
            Result<RegisteredUserService> res =  mySystem.Register(user.Email, user.Password);
            if (res.ExecStatus)
            {
                //res.Data.ShoppingCart = null;

            }
            return Ok(res);
        }

        /// <summary>
        /// search store by attributes
        /// {name: someName, rating:someRating}
        /// </summary>
        /// <param name="storeDetails"></param>
        /// <returns></returns>
        [Route("SearchStore")]
        [HttpGet]
        public Result<List<StoreService>> SearchStore([FromBody] IDictionary<string, object> storeDetails)
        {
            return mySystem.SearchStore(storeDetails);
        }

        /// <summary>
        /// Search product by attributes
        /// {name,category,lowprice,highprice,productrating,storerating,keywords}
        /// </summary>
        /// <param name="productDetails"></param>
        /// <returns></returns>
        [Route("SearchProduct")]
        [HttpGet]
        public Result<List<ProductService>> SearchProduct(IDictionary<string, object> productDetails)
        {
            return mySystem.SearchProduct(productDetails);
        }

        [Route("AddProductToCart")]
        [HttpPost]
        public Result<Boolean> AddProductToCart([FromBody] ProductToCart productToCart)
        {
            return mySystem.AddProductToCart(productToCart.userID, productToCart.ProductID, productToCart.ProductQuantity, productToCart.StoreID);
        }

        [Route("GetUserShoppingCart/{userID}")]
        [HttpGet]
        public IActionResult GetUserShoppingCart(String userID)
        {
            Result<ShoppingCartService> res = mySystem.GetUserShoppingCart(userID);
            return Ok(res);
        }

        //Result<Boolean> UpdateShoppingCart(String userID, String shoppingBagID, String productID, int quantity);
        //Result<ShoppingCartDAL> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails);
        //Result<HistoryDAL> GetUserPurchaseHistory(String userID);
        //Result<double> GetTotalShoppingCartPrice(String userID);
        //Result<ConcurrentDictionary<String, String>> GetProductReview(String storeID, String productID);

    }
}
