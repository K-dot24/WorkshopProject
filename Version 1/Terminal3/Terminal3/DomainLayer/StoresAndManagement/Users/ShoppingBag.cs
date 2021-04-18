using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DALobjects;
using System;
using System.Collections.Concurrent;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    // Association Class
    public class ShoppingBag
    {
        public String Id { get; }
        public User User { get; }
        public Store Store { get; }
        public ConcurrentDictionary<Product, int> Products { get; }     // <Product, Quantity>

        public ShoppingBag(User user , Store store)
        {
            Id = Service.GenerateId();
            User = user;
            Store = store;
            Products = new ConcurrentDictionary<Product, int>();
        }

        public Result<bool> AddProtuctToShoppingBag(Product product, int quantity)
        {
            Products.TryAdd(product, quantity);
            return new Result<bool>($"Product {product.Name} was added successfully to shopping bag of {Store.Name}\n", true, true);
        }

        public Result<ShoppingBagDAL> GetDAL()
        {
            RegisteredUserDAL user = (RegisteredUser)User.GetDAL().Data;
            StoreDAL store = Store.GetDAL().Data;
            LinkedList<ProductDAL> products = new LinkedList<ProductDAL>();
            foreach(Product p in Products)
            {
                products.AddLast(p.GetDAL().Data);
            }

            return new Result<ShoppingBagDAL>("Shopping bag DAL object", true, new ShoppingBagDAL(user, store, products));
        }

    }
}
