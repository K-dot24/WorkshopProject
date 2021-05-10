using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.ServiceLayer.Controllers
{
    public interface IDataController
    {
        List<StoreService> GetAllStoresToDisplay();
        List<ProductService> GetAllProductByStoreIDToDisplay(string storeID);
        Boolean[] GetPermission(string userID, string storeID);

    }
    /// <summary>
    /// Controller for getting all the data in order to display in on the web.
    /// This controller does not hold any functunality for the user to preform,
    /// for that reason no need to wrap with logic component as "Result"
    /// </summary>
    public class DataController : IDataController
    {
        //Properties
        public IStoresAndManagementInterface StoresAndManagementInterface { get; }

        //Constructor
        public DataController(IStoresAndManagementInterface storesAndManagementInterface)
        {
            this.StoresAndManagementInterface = storesAndManagementInterface;
        }

        #region Methods
        public List<StoreService> GetAllStoresToDisplay()
        {
            return StoresAndManagementInterface.GetAllStoresToDisplay();
        }
        public List<ProductService> GetAllProductByStoreIDToDisplay(string storeID)
        {
            return StoresAndManagementInterface.GetAllProductByStoreIDToDisplay(storeID);
        }

        public Boolean[] GetPermission(string userID, string storeID)
        {
            return StoresAndManagementInterface.GetPermission(userID, storeID);
        }


        #endregion
    }
}
