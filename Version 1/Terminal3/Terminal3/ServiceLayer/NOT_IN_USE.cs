using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.ServiceLayer
{
    public interface NOT_IN_USE
    {
        #region System related operations
        Result<Object> ResetSystem();
        #endregion

        #region User related operations
        Result<Object> Register(String email , String password);         
        Result<Object> Login(String email, String password);
        Result<Object> LogOut(String email);
        Result<Object> SearchStore(IDictionary<String,Object> details);
        Result<Object> SearchProduct(IDictionary<String, Object> details);
        Result<Object> AddProductToCart(User user, Product product);   // Redundent ?
        Result<Object> GetUserShoppingCart(User user);
        Result<Object> UpdateShoppingCart(User user , Product product , int quantity);
        Result<Object> Purchase(User user);
        Result<Object> GetUserPurchaseHistory(User user);
        Result<Object> GetTotalShoppingCartPrice(User user);
        #endregion 

        #region Store related operations
        Result<Object> OpenNewStore(String storeName,User user);
        Result<Object> AddProductToStore(User user, Store store, Product product, int quantity);
        Result<Object> RemoveProductFromStore(User user, Store store, Product product);
        Result<Object> EditProductDetails(User user, Store store, Product product, IDictionary<String, Object> details);
        Result<Object> SetPurchasePolicyAtStore(User user, Store store, IPurchasePolicy policy);
        Result<Object> GetPurchasePolicyAtStore(User user, Store store);
        Result<Object> SetDiscountPolicyAtStore(User user, Store store, IDiscountPolicy policy);
        Result<Object> GetDiscountPolicyAtStore(User user, Store store);
        Result<Object> AddStoreOwner(User addedOwner,User currentlyOwner ,Store store);
        Result<Object> AddStoreManager(User addedManager,User currentlyOwner ,Store store);
        Result<Object> RemoveStoreManager(User addedManager,User currentlyOwner ,Store store);
        Result<Object> SetPermissions(User manager,User owner ,Permission permissions);
        Result<Object> GetStoreStaff(User owner ,Store store);
        Result<Object> GetStorePurchaseHistory(User owner ,Store store);
        #endregion

    }
}
