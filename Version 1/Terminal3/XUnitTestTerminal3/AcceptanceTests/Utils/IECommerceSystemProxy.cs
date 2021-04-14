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

        public Result<Object> AddProductToCart(User user, Product product)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.AddProductToCart(user, product);
        }

        public Result<Object> AddProductToStore(User user, Store store, Product product, int quantity)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.AddProductToStore(user, store, product, quantity);
        }

        public Result<Object> AddStoreManager(User addedManager, User currentlyOwner, Store store)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.AddStoreManager(addedManager, currentlyOwner, store);
        }

        public Result<Object> AddStoreOwner(User addedOwner, User currentlyOwner, Store store)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.AddStoreOwner(addedOwner, currentlyOwner, store);
        }

        public Result<Object> EditProductDetails(User user, Store store, Product product, IDictionary<string, object> details)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.EditProductDetails(user, store, product, details);
        }

        public Result<Object> GetDiscountPolicyAtStore(User user, Store store)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.GetDiscountPolicyAtStore(user, store);
        }

        public Result<Object> GetPurchasePolicyAtStore(User user, Store store)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.GetPurchasePolicyAtStore(user, store);
        }

        public Result<Object> GetStorePurchaseHistory(User owner, Store store)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.GetStorePurchaseHistory(owner, store);
        }

        public Result<Object> GetStoreStaff(User owner, Store store)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.GetStoreStaff(owner, store);
        }

        public Result<Object> GetTotalShoppingCartPrice(User user)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.GetTotalShoppingCartPrice(user);
        }

        public Result<Object> GetUserPurchaseHistory(User user)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.GetUserPurchaseHistory(user);
        }

        public Result<Object> GetUserShoppingCart(User user)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.GetUserShoppingCart(user);
        }

        public Result<Object> Login(string email, string password)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.Login(email, password);
        }

        public Result<Object> LogOut(string email, string password)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.LogOut(email, password);
        }

        public Result<Object> OpenNewStore(string storeName, User user)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.OpenNewStore(storeName, user);
        }

        public Result<Object> Purchase(User user)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.Purchase(user);
        }

        public Result<Object> Register(string email, string password)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.Register(email, password);
        }

        public Result<Object> RemoveProductFromStore(User user, Store store, Product product)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.RemoveProductFromStore(user, store, product);
        }

        public Result<Object> RemoveStoreManager(User addedManager, User currentlyOwner, Store store)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.RemoveStoreManager(addedManager, currentlyOwner, store);
        }

        public Result<Object> ResetSystem()
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.ResetSystem();
        }

        public Result<Object> SearchProduct(IDictionary<string, object> details)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.SearchProduct(details);
        }

        public Result<Object> SearchStore(IDictionary<string, object> details)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.SearchStore(details);
        }

        public Result<Object> SetDiscountPolicyAtStore(User user, Store store, IDiscountPolicy policy)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.SetDiscountPolicyAtStore(user, store, policy);
        }

        public Result<Object> SetPermissions(User manager, User owner, Permission permissions)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.SetPermissions(manager, owner, permissions);
        }

        public Result<Object> SetPurchasePolicyAtStore(User user, Store store, IPurchasePolicy policy)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.SetPurchasePolicyAtStore(user, store, policy);
        }

        public Result<Object> UpdateShoppingCart(User user, Product product, int quantity)
        {
            if (Real == null)
                return new Result<Object>(true);

            return Real.UpdateShoppingCart(user, product, quantity);
        }
    }
}
