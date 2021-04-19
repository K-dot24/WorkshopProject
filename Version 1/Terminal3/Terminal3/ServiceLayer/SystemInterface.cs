﻿using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.DALobjects;
using System.Collections.Concurrent;


namespace Terminal3.ServiceLayer
{
    //For Testing 
    public interface ISystemInterface
    {

        #region System related operations
        Result<Boolean> ResetSystem();
        #endregion

        #region User related operations
        Result<Boolean> Register(String email, String password);

        Result<String> Login(String email, String password); //user id

        Result<Boolean> LogOut(String email);

        //TODO: refine requirement
        Result<Object> SearchStore(IDictionary<String, Object> details);

        Result<List<String>> SearchProduct(IDictionary<String, Object> productDetails); //list<pid>
        
        Result<Boolean> AddProductToCart(String userID , String productID, int productQuantity , String storeID);

        Result<List<String>> GetUserShoppingCart(String userID); //LinkedList<ShoppingBagID> 

        Result<Dictionary<String, int>> GetUserShoppingBag(String userID, String shoppingBagID); //Dictionary<pid , countity>
        
        Result<Boolean> UpdateShoppingCart(String userID, String shoppingBagID , String productID , int quantity);

        Result<Object> Purchase(String userID , IDictionary<String, Object> paymentDetails , IDictionary<String, Object> deliveryDetails);

        Result<List<String>> GetUserPurchaseHistory(String userID); //List<shoppingBagID>

        Result<double> GetTotalShoppingCartPrice(String userID);

        Result<Boolean> AddProductReview(String userID, String storeID, String productID , String review);
        #endregion

        #region Store related operations
        Result<String> OpenNewStore(String storeName, String userID); //store id
        Result<String> AddProductToStore(String userID, String storeID, String productName , double price, int initialQuantity, String category ); //product id
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<String> EditProductDetails(String userID, String storeID , String productID, IDictionary<String, Object> details); //TODO
        Result<Boolean> AddStoreOwner(String addedOwnerID, String currentlyOwnerID, String storeID); 
        Result<Boolean> AddStoreManager(String addedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> RemoveStoreManager(String removedManagerID, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Boolean> RemovePermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<String, List<int>>> GetStoreStaff(String ownerID, String storeID);
        Result<List<String>> GetStorePurchaseHistory(String ownerID, String storeID); //userID to List<permissions>
        Result<ConcurrentDictionary<String, String>> GetProductReview(String storeID, String productID);

        #endregion


    }
}
