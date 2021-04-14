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
            if (Real == null)
                return new Result<bool>(true);

            return Real.AddProductToCart(user, product);
        }

        public Result<bool> AddProductToStore(User user, Store store, Product product, int quantity)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.AddProductToStore(user, store, product, quantity);
        }

        public Result<bool> AddStoreManager(User addedManager, User currentlyOwner, Store store)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.AddStoreManager(addedManager, currentlyOwner, store);
        }

        public Result<bool> AddStoreOwner(User addedOwner, User currentlyOwner, Store store)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.AddStoreOwner(addedOwner, currentlyOwner, store);
        }

        public Result<bool> EditProductDetails(User user, Store store, Product product, IDictionary<string, object> details)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.EditProductDetails(user, store, product, details);
        }

        public Result<bool> GetDiscountPolicyAtStore(User user, Store store)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.GetDiscountPolicyAtStore(user, store);
        }

        public Result<bool> GetPurchasePolicyAtStore(User user, Store store)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.GetPurchasePolicyAtStore(user, store);
        }

        public Result<bool> GetStorePurchaseHistory(User owner, Store store)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.GetStorePurchaseHistory(owner, store);
        }

        public Result<bool> GetStoreStaff(User owner, Store store)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.GetStoreStaff(owner, store);
        }

        public Result<bool> GetTotalShoppingCartPrice(User user)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.GetTotalShoppingCartPrice(user);
        }

        public Result<bool> GetUserPurchaseHistory(User user)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.GetUserPurchaseHistory(user);
        }

        public Result<bool> GetUserShoppingCart(User user)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.GetUserShoppingCart(user);
        }

        public Result<bool> Login(string email, string password)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.Login(email, password);
        }

        public Result<bool> LogOut(string email, string password)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.LogOut(email, password);
        }

        public Result<bool> OpenNewStore(string storeName, User user)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.OpenNewStore(storeName, user);
        }

        public Result<bool> Purchase(User user)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.Purchase(user);
        }

        public Result<bool> Register(string email, string password)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.Register(email, password);
        }

        public Result<bool> RemoveProductFromStore(User user, Store store, Product product)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.RemoveProductFromStore(user, store, product);
        }

        public Result<bool> RemoveStoreManager(User addedManager, User currentlyOwner, Store store)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.RemoveStoreManager(addedManager, currentlyOwner, store);
        }

        public Result<bool> ResetSystem()
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.ResetSystem();
        }

        public Result<bool> SearchProduct(IDictionary<string, object> details)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.SearchProduct(details);
        }

        public Result<bool> SearchStore(IDictionary<string, object> details)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.SearchStore(details);
        }

        public Result<bool> SetDiscountPolicyAtStore(User user, Store store, IDiscountPolicy policy)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.SetDiscountPolicyAtStore(user, store, policy);
        }

        public Result<bool> SetPermissions(User manager, User owner, Permission permissions)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.SetPermissions(manager, owner, permissions);
        }

        public Result<bool> SetPurchasePolicyAtStore(User user, Store store, IPurchasePolicy policy)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.SetPurchasePolicyAtStore(user, store, policy);
        }

        public Result<bool> UpdateShoppingCart(User user, Product product, int quantity)
        {
            if (Real == null)
                return new Result<bool>(true);

            return Real.UpdateShoppingCart(user, product, quantity);
        }
    }
}
