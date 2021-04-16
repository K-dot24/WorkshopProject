using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;

namespace Terminal3.ServiceLayer
{
    //Primitive-DAL oriented interface
    public interface ISystemInterface
    {

        #region System related operations
        Result<Boolean> ResetSystem();
        #endregion

        #region User related operations
        Result<Boolean> Register(String email, String password);

        Result<Boolean> Login(String email, String password);

        Result<Boolean> LogOut(String email);

        //TODO: refine requirement
        Result<Object> SearchStore(IDictionary<String, Object> details);

        Result<List<ProductDAL>> SearchProduct(IDictionary<String, Object> productDetails);
        
        Result<Boolean> AddProductToCart(String userID , String ProductID, int ProductQuantity , String StoreID);   // Redundent ?

        Result<ShoppingCartDAL> GetUserShoppingCart(String userID);

        Result<Boolean> UpdateShoppingCart(String userID, String shoppingBagID , String productID , int quantity);

        Result<Object> Purchase(String userID , IDictionary<String, Object> paymentDetails , IDictionary<String, Object> deliveryDetails);

        Result<HistoryDAL> GetUserPurchaseHistory(String userID);

        Result<int> GetTotalShoppingCartPrice(String userID);
        #endregion

        #region Store related operations
        Result<StoreDAL> OpenNewStore(String storeName, String userID);
        Result<ProductDAL> AddProductToStore(String userID, String storeID, String productName , double price, int initialQuantity, String category );
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<ProductDAL> EditProductDetails(String userID, String storeID , String productID, IDictionary<String, Object> details);
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID);
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<UserDAL , PermissionDAL>> GetStoreStaff(String ownerID, String storeID);
        Result<HistoryDAL> GetStorePurchaseHistory(String ownerID, String storeID);
        #endregion

 
    }
}
