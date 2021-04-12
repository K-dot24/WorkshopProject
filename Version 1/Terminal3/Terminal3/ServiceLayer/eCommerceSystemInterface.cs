using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies;
using Terminal3.DomainLayer.StoresAndManagment.Users;

namespace Terminal3.ServiceLayer
{
    public interface IECommerceSystemInterface
    {
        #region System related operations
        Result<Boolean> ResetSystem();
        #endregion

        #region User related operations
        Result<Boolean> Register(String email , String password);         
        Result<Boolean> Login(String email, String password);
        Result<Boolean> LogOut(String email, String password);
        Result<Boolean> SearchStore(IDictionary<String,Object> details);
        Result<Boolean> SearchProduct(IDictionary<String, Object> details);
        Result<Boolean> AddProductToCart(User user, Product product);   // Redundent ?
        Result<Boolean> GetUserShoppingCart(User user);
        Result<Boolean> UpdateShoppingCart(User user , Product product , int quantity);
        Result<Boolean> Purchase(User user);
        Result<Boolean> GetUserPurchaseHistory(User user);
        Result<Boolean> GetTotalShoppingCartPrice(User user);
        #endregion 

        #region Store related operations
        Result<Boolean> OpenNewStore(String storeName,User user);
        Result<Boolean> AddProductToStore(User user, Store store, Product product, int quantity);
        Result<Boolean> RemoveProductFromStore(User user, Store store, Product product);
        Result<Boolean> EditProductDetails(User user, Store store, Product product, IDictionary<String, Object> details);
        Result<Boolean> SetPurchasePolicyAtStore(User user, Store store, IPurchasePolicy policy);
        Result<Boolean> GetPurchasePolicyAtStore(User user, Store store);
        Result<Boolean> SetDiscountPolicyAtStore(User user, Store store, IDiscountPolicy policy);
        Result<Boolean> GetDiscountPolicyAtStore(User user, Store store);
        Result<Boolean> AddStoreOwner(User addedOwner,User currentlyOwner ,Store store);
        Result<Boolean> AddStoreManager(User addedManager,User currentlyOwner ,Store store);
        Result<Boolean> RemoveStoreManager(User addedManager,User currentlyOwner ,Store store);
        Result<Boolean> SetPermissions(User manager,User owner ,Permission permissions);
        Result<Boolean> GetStoreStaff(User owner ,Store store);
        Result<Boolean> GetStorePurchaseHistory(User owner ,Store store);
        #endregion

    }
}
