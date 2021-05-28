using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.ServiceLayer.ServiceObjects;
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
        public Double TotalBagPrice { get; set; }

        public ShoppingBag(User user , Store store)
        {
            Id = Service.GenerateId();
            User = user;
            Store = store;
            Products = new ConcurrentDictionary<Product, int>();
            TotalBagPrice = 0;
        }

        // For loading from database
        public ShoppingBag(String id , User user, Store store , ConcurrentDictionary<Product, int> products , Double totalBagPrice)
        {
            Id = id;
            User = user;
            Store = store;
            Products = products;
            TotalBagPrice = totalBagPrice;
        }

        //TODO - delete - for testing
        public ShoppingBag(String id, User user, ConcurrentDictionary<Product, int> products, Double totalBagPrice)
        {
            Id = id;
            User = user;
            Store = null;
            Products = products;
            TotalBagPrice = totalBagPrice;
        }

        public Result<bool> AddProtuctToShoppingBag(Product product, int quantity)
        {
            if (product.Quantity >= quantity && quantity > 0)
            {
                Products.TryAdd(product, quantity);
                this.TotalBagPrice = GetTotalPrice();
                return new Result<bool>($"Product {product.Name} was added successfully to shopping bag of {Store.Name}\n", true, true);
            }
            //else failed
            return new Result<bool>($"Asked quantity ({quantity}) of product {product.Name} is higher than quantity in store ({product.Quantity}).\n", false, false);
        }

        // this quantity will be the updated quantity of the product in the bag .
        // if negative or zero then the product will be removed
        public Result<Boolean> UpdateShoppingBag(Product product, int quantity)
        {
            if(Products.ContainsKey(product))
            {
                if (quantity <= 0)
                {
                    Products.Remove(product, out int q);
                    return new Result<Boolean>($"The product {product.Name} was removed from shopping bag successfuly\n", true, true);
                }
                
                if(product.Quantity >= quantity)
                {
                    bool getCurrquantity = Products.TryGetValue(product, out int currQuantity);
                    bool update = Products.TryUpdate(product, quantity, currQuantity);
                    if(getCurrquantity && update)
                    {
                        return new Result<Boolean>($"The product {product.Name} quantity was updated successfuly\n", true, true);
                    }
                    //else faild
                    return new Result<Boolean>("Attempt to update shopping cart faild\n", false, false);
                }
                //else faild
                return new Result<Boolean>($"Asked quantity ({quantity}) of product {product.Name} is higher than quantity in store ({product.Quantity}).\n", false, false);
            }
            //else faild
            return new Result<Boolean>($"You did not add the product {product.Name} to this shopping bag. Therefore attempt to update shopping bag faild\n", false, false);
        }
        public Result<ShoppingBagService> GetDAL()
        {
            ConcurrentDictionary<ProductService, int> products = new ConcurrentDictionary<ProductService, int>();
            foreach (var p in Products)
            {
                products.TryAdd(p.Key.GetDAL().Data, p.Value);                    
            }
            return new Result<ShoppingBagService>("Shopping bag DAL object", true, new ShoppingBagService(Id , User.Id, Store.Id, products , TotalBagPrice));
        }

        internal double GetTotalPrice(String DiscountCode = "")
        {
            Double amount = Store.PolicyManager.GetTotalBagPrice(this.Products, DiscountCode);
            this.TotalBagPrice = amount; 

            return amount; 
        }

        internal Result<bool> AdheresToPolicy()
        {
            return Store.PolicyManager.AdheresToPolicy(this.Products, this.User);
        }
    }
}
