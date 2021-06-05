using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.ServiceLayer;
using Terminal3.ServiceLayer.Controllers;
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
        /// Template of valid JSON:
        /// {
        ///     "userID":"string",
        ///     "storeID":"string",
        ///     "productName":"string,
        ///     "price":double,
        ///     "initialQuantity":int,
        ///     "category":"string",
        ///     "keywords":["string","string"],
        /// }
        /// </summary>
        /// <param name="data"></param>
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
        ///     "userID":"string",
        ///     "storeID:"string",
        ///     "productID":"string",
        ///     "details": {
        ///                     "Name":"string",
        ///                     "Price":double,
        ///                     "Quantity":int,
        ///                     "Category":"string",
        ///                     "Keywords":["string","string"...]
        ///                 }
        /// }
        /// NOTE: all fields in "detalis" values are optionals
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("EditProductDetails")]
        [HttpPut]
        public IActionResult EditProductDetails([FromBody] EditProductDetailsModel data)
        {
            Result<ProductService> result = system.EditProductDetails(data.userID, data.storeID, data.productID, data.details);
            if (result.ExecStatus) { return Ok(result.Message); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// Add new store owner to a given store
        /// Template of valid JSON:
        /// {
        ///     "addedOwnerID":"string",
        ///     "currentlyOwnerID:"string",
        ///     "storeID":"string"
        /// }
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("AddStoreOwner")]
        [HttpPost]
        public IActionResult AddStoreOwner([FromBody] AddStoreOwnerModel data)
        {
            Result<Boolean> result = system.AddStoreOwner(data.addedOwnerID, data.currentlyOwnerID, data.storeID);
            if (result.ExecStatus) { return Created("", result.Message); }
            else { return BadRequest(result.Message); }
        }
        /// <summary>
        /// Adding new store manager to a given store
        /// Template of valid JSON:
        /// {
        ///     "addedManagerID":"string",
        ///     "currentlyOwnerID:"string",
        ///     "storeID":"string"
        /// }
        /// </summary>
        /// <param name="data"></param>
        [Route("AddStoreManager")]
        [HttpPost]
        public IActionResult AddStoreManager([FromBody] AddStoreManagerModel data)
        {
            Result<Boolean> result = system.AddStoreManager(data.addedManagerID, data.currentlyOwnerID, data.storeID);
            if (result.ExecStatus) { return Created("", result.Message); }
            else { return BadRequest(result.Message); }
        }
        /// <summary>
        /// Setting new set of permissions to manager
        /// Template of valid JSON:
        /// {
        ///     "storeID":"string",
        ///     "managerID:"string",
        ///     "ownerID":"string",
        ///     "permissions":[int,int,int...]
        /// }
        /// </summary>
        /// <param name="data"></param>
        [Route("SetPermissions")]
        [HttpPut]
        public IActionResult SetPermissions([FromBody] SetPermissionsModel data)
        {
            Result<Boolean> result = system.SetPermissions(data.storeID,data.managerID,data.ownerID,data.permissions);
            if (result.ExecStatus) { return Created("", result.Message); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// removing set of permissions to manager
        /// Template of valid JSON:
        /// {
        ///     "storeID":"string",
        ///     "managerID:"string",
        ///     "ownerID":"string",
        ///     "permissions":[int,int,int...]
        /// }
        /// </summary>
        /// <param name="data"></param>
        [Route("RemovePermissions")]
        [HttpPut]
        public IActionResult RemovePermissions([FromBody] SetPermissionsModel data)
        {
            Result<Boolean> result = system.RemovePermissions(data.storeID, data.managerID, data.ownerID, data.permissions);
            if (result.ExecStatus) { return Ok(result.Message); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// Return list of pair, each pair hold details about the store staff and its permissions
        /// </summary>
        /// <param name="ownerID">ID of the owner who request to preform the operation</param>
        /// <param name="storeID">storeID</param>
        /// <returns></returns>
        [Route("GetStoreStaff/{ownerID}/{storeID}")]
        [HttpGet]
        public IActionResult GetStoreStaff(String ownerID, String storeID)
        {
            Result<List<Tuple<IStoreStaffService, PermissionService>>> result = system.GetStoreStaff(ownerID, storeID);
            if (result.ExecStatus) { return Ok(result.Data); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// Returns in-store purchase history
        /// </summary>
        /// <param name="ownerID">ownerID</param>
        /// <param name="storeID">ID of the store to get the purchase history</param>
        /// <returns></returns>
        [Route("GetStorePurchaseHistory/{sysAdminID}/{storeId}")]
        [HttpGet]
        public IActionResult GetStorePurchaseHistory(String ownerID, String storeID)
        {
            Result<HistoryService> result = system.GetStorePurchaseHistory(ownerID, storeID, false);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// Removing excisting store manager by an owner
        /// </summary>
        /// <param name="storeID">StoreID</param>
        /// <param name="currentlyOwnerID">OwnerID</param>
        /// <param name="removedManagerID">ID of the manager to be removed</param>
        /// <returns></returns>
        [Route("RemoveStoreManager/{storeID}/{currentlyOwnerID}/{removedManagerID}")]
        [HttpDelete]
        public IActionResult RemoveStoreManager(string storeID, string currentlyOwnerID, string removedManagerID)
        {
            Result<Boolean> result = system.RemoveStoreManager(removedManagerID,currentlyOwnerID,storeID);
            if (result.ExecStatus) { return Ok(result.Message); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// Removing excisting store owner by an owner
        /// </summary>
        /// <param name="storeID">StoreID</param>
        /// <param name="currentlyOwnerID">OwnerID</param>
        /// <param name="removedOwnerID">ID of the manager to be removed</param>
        [Route("RemoveStoreOwner/{storeID}/{currentlyOwnerID}/{removedOwnerID}")]
        [HttpDelete]
        public IActionResult RemoveStoreOwner(string removedOwnerID, string currentlyOwnerID, string storeID)
        {
            Result<Boolean> result = system.RemoveStoreOwner(removedOwnerID, currentlyOwnerID, storeID);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }

        }

        /// <summary>
        /// Closing an active store 
        /// </summary>
        /// <param name="storeId">storeId</param>
        /// <param name="userID">userID</param>
        [Route("CloseStore/{storeId}/{userID}")]
        [HttpDelete]
        public IActionResult CloseStore(string storeId, string userID)
        {
            Result<Boolean> result = system.CloseStore(storeId, userID);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// ReOpenStore by storeID
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route("ReOpenStore/{storeId}/{userID}")]
        [HttpPost]
        public IActionResult ReOpenStore(string storeId, string userID)
        {
            Result<StoreService> result = system.ReOpenStore(storeId, userID);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }

        }

        [Route("AddDiscountPolicy")]
        [HttpPost]
        public IActionResult AddDiscountPolicy([FromBody] PolicyModel data)
        {
            Result<Boolean> result = system.AddDiscountPolicy(data.storeId, data.info);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        [Route("AddDiscountPolicy/{id}")]
        [HttpPost]
        public IActionResult AddDiscountPolicy([FromBody] PolicyModel data,String id)
        {
            Result<Boolean> result = system.AddDiscountPolicy(data.storeId, data.info,id);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        [Route("AddDiscountCondition/{id}")]
        [HttpPost]
        public IActionResult AddDiscountCondition([FromBody] PolicyModel data, String id)
        {
            Result<Boolean> result = system.AddDiscountCondition(data.storeId, data.info, id);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        [Route("RemoveDiscountPolicy/{storeId}/{id}")]
        [HttpDelete]
        public IActionResult RemoveDiscountPolicy(string storeId, String id)
        {
            Result<Boolean> result = system.RemoveDiscountPolicy(storeId, id);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        [Route("RemoveDiscountCondition/{storeId}/{id}")]
        [HttpDelete]
        public IActionResult RemoveDiscountCondition(string storeId, String id)
        {
            Result<Boolean> result = system.RemoveDiscountCondition(storeId, id);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        [Route("EditDiscountPolicy/{id}")]
        [HttpPut]
        public IActionResult EditDiscountPolicy([FromBody] PolicyModel data ,String id)
        {
            Result<Boolean> result = system.EditDiscountPolicy(data.storeId,data.info, id);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        [Route("EditDiscountCondition/{id}")]
        [HttpPut]
        public IActionResult EditDiscountCondition([FromBody] PolicyModel data, String id)
        {
            Result<Boolean> result = system.EditDiscountCondition(data.storeId, data.info, id);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        [Route("GetDiscountPolicyData/{storeId}")]
        [HttpGet]
        public IActionResult GetDiscountPolicyData(string storeId)
        {
            Result<IDictionary<string, object>> result = system.GetDiscountPolicyData(storeId);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }
        [Route("GetPurchasePolicyData/{storeId}")]
        [HttpGet]
        public IActionResult GetPurchasePolicyData(string storeId)
        {
            Result<IDictionary<string, object>> result = system.GetPurchasePolicyData(storeId);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        [Route("AddPurchasePolicy")]
        [HttpPost]
        public IActionResult AddPurchasePolicy([FromBody] PolicyModel data)
        {
            Result<bool> result = system.AddPurchasePolicy(data.storeId, data.info);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        [Route("AddPurchasePolicy/{id}")]
        [HttpPost]
        public IActionResult AddPurchasePolicy([FromBody] PolicyModel data, String id)
        {
            Result<bool> result = system.AddPurchasePolicy(data.storeId, data.info,id);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }


        [Route("RemovePurchasePolicy/{storeId}/{id}")]
        [HttpDelete]
        public IActionResult RemovePurchasePolicy(string storeId, String id)
        {
            Result<bool> result = system.RemovePurchasePolicy(storeId, id);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }


        [Route("EditPurchasePolicy/{id}")]
        [HttpPut]
        public IActionResult EditPurchasePolicy([FromBody] PolicyModel data, string id)
        {
            Result<bool> result = system.EditPurchasePolicy(data.storeId, data.info, id);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }
        /// Getting income for the store by 2 dates interval
        /// Template of valid JSON:
        /// {
        ///     "StartDate":"string",
        ///     "EndDate:"string",
        ///     "StoreID":"string",
        ///     "OwnerID":"string"
        /// }
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("GetIncomeAmountGroupByDay")]
        [HttpPost]
        public IActionResult GetIncomeAmountGroupByDay([FromBody] GetIncomeAmountGroupByDayModel data)
        {
            Result<List<Tuple<DateTime, Double>>> result = system.GetIncomeAmountGroupByDay(data.StartDate,data.EndDate,data.StoreID,data.OwnerID);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }
        #endregion
    }
}
