using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer;


namespace XUnitTestTerminal3.IntegrationTests
{
    public class IECommerceSystemProxy : IECommerceSystemInterface
    {
        public IECommerceSystemInterface Real { get; set; }

        public Result<bool> AddProductToCart(User user, Product product)
        {
            return Real.AddProductToCart(user, product);
        }

        public Result<bool> AddProductToStore(User user, Store store, Product product, int quantity)
        {
            return Real.AddProductToStore(user, store, product, quantity);
        }

        public Result<bool> AddStoreManager(User addedManager, User currentlyOwner, Store store)
        {
            return Real.AddStoreManager(addedManager, currentlyOwner, store);
        }

        public Result<bool> AddStoreOwner(User addedOwner, User currentlyOwner, Store store)
        {
            return Real.AddStoreOwner(addedOwner, currentlyOwner, store);
        }

        public Result<bool> EditProductDetails(User user, Store store, Product product, IDictionary<string, object> details)
        {
            return Real.EditProductDetails(user, store, product, details);
        }

        public Result<bool> GetDiscountPolicyAtStore(User user, Store store)
        {
            return Real.GetDiscountPolicyAtStore(user, store);
        }

        public Result<bool> GetPurchasePolicyAtStore(User user, Store store)
        {
            return Real.GetPurchasePolicyAtStore(user, store);
        }

        public Result<bool> GetStorePurchaseHistory(User owner, Store store)
        {
            return Real.GetStorePurchaseHistory(owner, store);
        }

        public Result<bool> GetStoreStaff(User owner, Store store)
        {
            return Real.GetStoreStaff(owner, store);
        }

        public Result<bool> GetTotalShoppingCartPrice(User user)
        {
            return Real.GetTotalShoppingCartPrice(user);
        }

        public Result<bool> GetUserPurchaseHistory(User user)
        {
            return Real.GetUserPurchaseHistory(user);
        }

        public Result<bool> GetUserShoppingCart(User user)
        {
            return Real.GetUserShoppingCart(user);
        }

        public Result<bool> Login(string email, string password)
        {
            return Real.Login(email, password);
        }

        public Result<bool> LogOut(string email, string password)
        {
            return Real.LogOut(email, password);
        }

        public Result<bool> OpenNewStore(string storeName, User user)
        {
            return Real.OpenNewStore(storeName, user);
        }

        public Result<bool> Purchase(User user)
        {
            return Real.Purchase(user);
        }

        public Result<bool> Register(string email, string password)
        {
            return Real.Register(email, password);
        }

        public Result<bool> RemoveProductFromStore(User user, Store store, Product product)
        {
            return Real.RemoveProductFromStore(user, store, product);
        }

        public Result<bool> RemoveStoreManager(User addedManager, User currentlyOwner, Store store)
        {
            return Real.RemoveStoreManager(addedManager, currentlyOwner, store);
        }

        public Result<bool> ResetSystem()
        {
            return Real.ResetSystem();
        }

        public Result<bool> SearchProduct(IDictionary<string, object> details)
        {
            return Real.SearchProduct(details);
        }

        public Result<bool> SearchStore(IDictionary<string, object> details)
        {
            return Real.SearchStore(details);
        }

        public Result<bool> SetDiscountPolicyAtStore(User user, Store store, IDiscountPolicy policy)
        {
            return Real.SetDiscountPolicyAtStore(user, store, policy);
        }

        public Result<bool> SetPermissions(User manager, User owner, Permission permissions)
        {
            return Real.SetPermissions(manager, owner, permissions);
        }

        public Result<bool> SetPurchasePolicyAtStore(User user, Store store, IPurchasePolicy policy)
        {
            return Real.SetPurchasePolicyAtStore(user, store, policy);
        }

        public Result<bool> UpdateShoppingCart(User user, Product product, int quantity)
        {
            return Real.UpdateShoppingCart(user, product, quantity);
        }
    }
}
