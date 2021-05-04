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
    /// API controller for all the store staff functunality
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StoreStaffController : ControllerBase
    {
        //Fields
        private readonly IECommerceSystem system;

        //Constructor
        public StoreStaffController(IECommerceSystem system)
        {
            this.system = system;
        }

        #region End-Points
        /// <summary>
        /// Add product to store
        /// </summary>
        /// <param name="data">userID,storeID,productName,price,initialQuantity,category,keywords packed in model</param>
        [Route("AddProductToStore")]
        [HttpPost]
        public IActionResult AddProductToStore([FromBody] AddProductToStoreModel data)
        {
            Result<ProductService> result = system.AddProductToStore(data.userID,
                                                                    data.storeID,
                                                                    data.productName,
                                                                    data.price,
                                                                    data.initialQuantity,
                                                                    data.category);
             if (result.ExecStatus) { return Created("", result.Message); }
             else { return BadRequest(result.Message); }
        }
        /// <summary>
        /// Removing product from store
        /// </summary>
        /// <param name="userID">userId of the manager/owner who preform the action</param>
        /// <param name="storeID">storeId where the product is located in</param>
        /// <param name="productID">product identifier</param>
        /// <returns></returns>
        [Route("RemoveProductFromStore/{userID}/{storeID}/{productID}")]
        [HttpDelete]
        public IActionResult RemoveProductFromStore(String userID, String storeID, String productID)
        {
            Result<Boolean> result = system.RemoveProductFromStore(userID, storeID, productID);
            if (result.ExecStatus) { return Ok(result.Message); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// Edit product details
        /// Template of valid JSON:
        /// {
        ///     "userID":"1",
        ///     "storeID:"1",
        ///     "productID":"1",
        ///     "details": {
        ///                     "Name":"samplename",
        ///                     "Price":10.0,
        ///                     "Quantity":12,
        ///                     "Category":"Fruit",
        ///                     "Keywords":["word1","word2"...]
        ///                 }
        /// }
        /// </summary>
        /// <param name="data">json object hold [userID,storeID,productID,dict of details]</param>
        /// <returns></returns>
        [Route("EditProductDetails")]
        [HttpPut]
        public IActionResult EditProductDetails([FromBody] EditProductDetailsModel data)
        {
            Result<ProductService> result = system.EditProductDetails(data.userID, data.storeID, data.productID, data.details);
            if (result.ExecStatus) { return Ok(result.Message); }
            else { return BadRequest(result.Message); }
        }
        [Route("AddStoreOwner")]
        [HttpPost]
        public IActionResult AddStoreOwner([FromBody] AddStoreOwnerModel data)
        {
            Result<Boolean> result = system.AddStoreOwner(data.addedOwnerID, data.currentlyOwnerID, data.storeID);
            if (result.ExecStatus) { return Created("", result.Message); }
            else { return BadRequest(result.Message); }
        }
        //public Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        //public Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        //public Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        //public Result<List<Tuple<IStoreStaffService, PermissionService>>> GetStoreStaff(String ownerID, String storeID);
        //public Result<HistoryService> GetStorePurchaseHistory(String ownerID, String storeID, Boolean isSystemAdmin = false);
        //public Result<Boolean> RemoveStoreManager(string removedManagerID, string currentlyOwnerID, string storeID);

        #endregion
    }
}
