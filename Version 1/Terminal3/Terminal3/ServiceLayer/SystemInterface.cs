using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.ServiceLayer.ServiceObjects;
using System.Collections.Concurrent;


namespace Terminal3.ServiceLayer
{
    //For Testing 
    public interface ISystemInterface
    {

        #region System related operations
        Result<Boolean> ResetSystem(string sysAdminID);
        #endregion

        #region User related operations
        Result<Boolean> Register(String email, String password);

        Result<String> Login(String email, String password); //user id

        Result<Boolean> LogOut(String email);

        //TODO: refine requirement
        Result<List<String>> SearchStore(IDictionary<String, Object> details); // store name

        Result<List<String>> SearchProduct(IDictionary<String, Object> productDetails); //list<pid>
        
        Result<Boolean> AddProductToCart(String userID , String productID, int productQuantity , String storeID);

        Result<List<String>> GetUserShoppingCart(String userID); //LinkedList<ShoppingBagID> 

        Result<Dictionary<string, int>> GetUserShoppingBag(String userID, String shoppingBagID); //Dictionary<pid , countity>

        Result<Boolean> UpdateShoppingCart(String userID, String storeId , String productID , int quantity);

        Result<List<String>> Purchase(String userID , IDictionary<String, Object> paymentDetails , IDictionary<String, Object> deliveryDetails); //List<ShoppingBagID>

        Result<List<String>> GetUserPurchaseHistory(String userID); //List<shoppingBagID>

        Result<double> GetTotalShoppingCartPrice(String userID);

        Result<Boolean> AddProductReview(String userID, String storeID, String productID , String review);
        #endregion

        #region Store related operations
        Result<String> OpenNewStore(String storeName, String userID); //store id
        Result<String> AddProductToStore(String userID, String storeID, String productName , double price, int initialQuantity, String category ); //product id
        Result<Boolean> RemoveProductFromStore(String userID, String storeID, String productID);
        Result<String> EditProductDetails(String userID, String storeID , String productID, IDictionary<String, Object> details); //TODO
        Result<Boolean> AddStoreOwner(String addedOwnerEmail, String currentlyOwnerID, String storeID); 
        Result<Boolean> AddStoreManager(String addedManagerEmail, String currentlyOwnerID, String storeID);
        Result<Boolean> SetPermissions(String storeID, String managerID, String ownerID, LinkedList<int> permissions);
        Result<Dictionary<String, List<int>>> GetStoreStaff(String ownerID, String storeID);
        Result<List<String>> GetStorePurchaseHistory(String ownerID, String storeID); //userID to List<permissions>

        #endregion
        #region Offer
        Result<bool> SendOfferToStore(string storeID, string userID, string productID, int amount, double price);
        Result<bool> AnswerCounterOffer(string userID, string offerID, bool accepted);
        Result<bool> SendOfferResponseToUser(string storeID, string ownerID, string userID, string offerID, bool accepted, double counterOffer);
        Result<List<Dictionary<string, object>>> getStoreOffers(string storeID);
        Result<List<Dictionary<string, object>>> getUserOffers(string userId);

        #endregion

    }
}
