using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Terminal3WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestUserController : ControllerBase
    {
        static String controllerRoute = "api/[controller]";

        #region old methods

        // GET api/<GuestUserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GuestUserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GuestUserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GuestUserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        #endregion

        [HttpGet]
        public string EnterSystem() { return "Welcome to Terminal3 system"; }

        [Route("ExitSystem/{userID:int}")]
        [HttpGet]
        public string ExitSystem(int userID) { return $"user {userID} Exit system"; }

        [Route("Register")]
        [HttpPost]
        public string Register(string email, string password) { return $"{email} register succesfuly"; }
        //Result<List<StoreDAL>> SearchStore(IDictionary<String, Object> details);
        //Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails);
        //Result<Boolean> AddProductToCart(String userID, String ProductID, int ProductQuantity, String StoreID);
        //Result<ShoppingCartDAL> GetUserShoppingCart(String userID);
        //Result<Boolean> UpdateShoppingCart(String userID, String shoppingBagID, String productID, int quantity);
        //Result<ShoppingCartDAL> Purchase(String userID, IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails);
        //Result<HistoryDAL> GetUserPurchaseHistory(String userID);
        //Result<double> GetTotalShoppingCartPrice(String userID);
        //Result<ConcurrentDictionary<String, String>> GetProductReview(String storeID, String productID);
    }
}
