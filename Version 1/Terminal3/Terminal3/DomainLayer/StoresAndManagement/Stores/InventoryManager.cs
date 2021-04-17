using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{

    public interface IInventoryManager
    {
        Result<Product> AddNewProduct(String productName, Double price, int initialQuantity, String category, LinkedList<String> keywords = null);
        Result<Product> RemoveProduct(String productID);
        Result<Product> EditProduct(String productID, IDictionary<String, Object> details);
    }

    public class InventoryManager : IInventoryManager
    {

        public ConcurrentDictionary<String, Product> Products { get; }
        
        //TODO: Change constructor if needed (Initializer?)
        public InventoryManager()
        {
            Products = new ConcurrentDictionary<String, Product>();
        }

        public InventoryManager(ConcurrentDictionary<String, Product> products)
        {
            Products = products;
        }

        public Result<Product> AddNewProduct(String productName, Double price, int initialQuantity, String category, LinkedList<String> keywords = null)
        {
            Product newProduct = new Product(productName, price, initialQuantity, category, keywords);
            return new Result<Product>($"Product {newProduct.Name} was created successfully. ID: {newProduct.Id}\n", true, newProduct);
        }

        public Result<Product> RemoveProduct(string productID)
        {
            if (Products.TryRemove(productID, out Product toRemove))
            {
                return new Result<Product>($"Product (ID: {toRemove.Id}) was removed successfully\n", true, toRemove);
            }
            //else
            return new Result<Product>($"Product (ID: {productID}) not found.\n", false, null);
        }

        public Result<Product> EditProduct(string productID, IDictionary<String, object> details)
        {
            if (Products.TryGetValue(productID, out Product toEdit))
            {
                ObjectDictionaryMapper<Product>.SetPropertyValue(toEdit, details);
            }
            //else failed
            return new Result<Product>($"Faild to edit product (ID: {productID}): Product not found.\n", false, null);
        }

        public Result<List<Product>> SearchProduct(Double StoreRating, ProductSearchAttributes searchAttributes)
        {
            List<Product> searchResults = new List<Product>();
            foreach(Product product in this.Products.Values)
            {
                if (searchAttributes.checkProduct(StoreRating,product))
                {
                    searchResults.Add(product);
                }
            }
            if (searchResults.Count > 0){
                return new Result<List<Product>>($"{searchResults.Count} items has been found\n", true, searchResults);
            }
            else{
                return new Result<List<Product>>($"No item has been found\n", false, null);
            }
        }
    }
}
