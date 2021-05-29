using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.DomainLayer;
using Terminal3.ServiceLayer;
using Terminal3.ServiceLayer.Controllers;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3WebAPI.Controllers
{
    /// <summary>
    /// API controller for all the system admins functunality
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdminController : ControllerBase
    {
        //Fields
        private readonly IECommerceSystem system;

        //Constructor
        public SystemAdminController(IECommerceSystem system)
        {
            this.system = system;
        }
        #region End-Points

        /// <summary>
        /// Returns user's purchase history
        /// </summary>
        /// <param name="sysAdminID">system admin ID</param>
        /// <param name="userID">ID of the user to get the purchase history</param>
        /// <returns></returns>
        [Route("GetUserPurchaseHistory/{sysAdminID}/{userID}")]
        [HttpGet]
        public IActionResult GetUserPurchaseHistory(string sysAdminID, string userID)
        {
            Result<HistoryService> result = system.GetUserPurchaseHistory(sysAdminID, userID);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// Returns in-store purchase history
        /// </summary>
        /// <param name="sysAdminID">system admin ID</param>
        /// <param name="storeId">ID of the store to get the purchase history</param>
        /// <returns></returns>
        [Route("GetStorePurchaseHistory/{sysAdminID}/{storeID}")]
        [HttpGet]
        public IActionResult GetStorePurchaseHistory(string sysAdminID, string storeId)
        {
            Result<HistoryService> result = system.GetStorePurchaseHistory(sysAdminID, storeId,true);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }
        /// <summary>
        /// Adding new system admin
        /// </summary>
        /// <param name="sysAdminID">userId of the system admin who preform the addition</param>
        /// <param name="email">email of the request new system admin</param>
        [Route("AddSystemAdmin/{sysAdminID}/{email}")]
        [HttpPost]
        public IActionResult AddSystemAdmin(string sysAdminID, string email)
        {
            Result<RegisteredUserService> result = system.AddSystemAdmin(sysAdminID, email);
            if (result.ExecStatus) { return Created($"api/registeruser/login/{result.Data.Email}", result.Data.Id); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// Removing existing system admin
        /// </summary>
        /// <param name="sysAdminID">userId of the system admin who preform the addition</param>
        /// <param name="email">email of admin to be removed</param>
        [Route("RemoveSystemAdmin/{sysAdminID}/{email}")]
        [HttpDelete]
        public IActionResult RemoveSystemAdmin(string sysAdminID, string email)
        {
            Result<RegisteredUserService> result = system.RemoveSystemAdmin(sysAdminID, email);
            if (result.ExecStatus) { return Ok(result.Message); }
            else { return BadRequest(result.Message); }
        }

        /// <summary>
        /// Reset the system, including all the stored data
        /// </summary>
        /// <param name="sysAdminID">userId of the system admin who preform the addition</param>
        [Route("ResetSystem/{sysAdminID}")]
        [HttpPut]
        public IActionResult ResetSystem(string sysAdminID)
        {
            Result<bool> result = system.ResetSystem(sysAdminID);
            if (result.ExecStatus) { return Ok(result.Message); }
            else { return BadRequest(result.Message); }
        }
        /// Getting income for the store by 2 dates interval
        /// NOTE!: OwnerID is the systemAdminID
        /// Template of valid JSON:
        /// {
        ///     "StartDate":"string",
        ///     "EndDate:"string",
        ///     "OwnerID":"string"
        /// }
        /// <param name="data"></param>
        [Route("GetIncomeAmountGroupByDay")]
        [HttpPost]
        public IActionResult GetIncomeAmountGroupByDay([FromBody] GetIncomeAmountGroupByDayModel data)
        {
            Result<List<Tuple<DateTime, Double>>> result = system.GetIncomeAmountGroupByDay(data.StartDate, data.EndDate, data.OwnerID);
            if (result.ExecStatus) { return Ok(result); }
            else { return BadRequest(result.Message); }
        }
        #endregion
    }
}
