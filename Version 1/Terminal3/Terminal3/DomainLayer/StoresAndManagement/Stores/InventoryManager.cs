using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{

    public interface IInventoryManager
    {
        Result<Product> AddNewProduct(String productName, Double price, int initialQuantity, String category, LinkedList<String> keywords = null);
        Result<Product> RemoveProduct(String productID);
        Result<Product> EditProduct(String productID, IDictionary<String, Object> details);
        Result<ConcurrentDictionary<String, String>> GetProductReview(String productID);
    }

    public class InventoryManager : IInventoryManager
    {

        public ConcurrentDictionary<String, Product> Products { get; }
        
        //TODO: Change constructor if needed (Initializer?)
        public InventoryManager()
        {
            Products = new ConcurrentDictionary<String, Product>();
            createProducts();
        }

        public InventoryManager(ConcurrentDictionary<String, Product> products)
        {
            Products = products;
        }

        public void createProducts()
        {
            AddNewProduct("p1", 10, 10, "test1", null);
            AddNewProduct("p2", 20, 10, "test2", null);
            AddNewProduct("p3", 30, 10, "test3", null);
            AddNewProduct("p4", 40, 10, "test4", null);
        }


        public Result<Product> AddNewProduct(String productName, Double price, int initialQuantity, String category, LinkedList<String> keywords = null)
        {
            Product newProduct = new Product(productName, price, initialQuantity, category, keywords);
            Products.TryAdd(newProduct.Id, newProduct);
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
                return new Result<Product>($"Succeded to edit product (ID: {productID}).\n", true, toEdit);
            }
            //else failed
            return new Result<Product>($"Failed to edit product (ID: {productID}): Product not found.\n", false, null);
        }

        public Result<List<Product>> SearchProduct(Double StoreRating, IDictionary<String,Object> searchAttributes)
        {
            List<Product> searchResults = new List<Product>();
            foreach(Product product in this.Products.Values)
            {
                if (checkProduct(StoreRating,product,searchAttributes))
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

        public Result<ConcurrentDictionary<String, String>> GetProductReview(String productID)
        {
            if(Products.TryGetValue(productID , out Product product))
            {
                return product.GetProductReview();
            }
            return new Result<ConcurrentDictionary<string, string>>("Product does not exists in the store\n", false, null);

        }

        /// <summary>
        ///  Filter out product if its not meet the search criteria
        /// </summary>
        /// <param name="StoreRating"></param>
        /// <param name="product"></param>
        /// <param name="searchAttributes"></param>
        /// <returns></returns>
        internal bool checkProduct(Double StoreRating, Product product, IDictionary<String, Object> searchAttributes)
        {
            Boolean result = true;
            ICollection<String> properties = searchAttributes.Keys;
            foreach (string property in properties)
            {
                var value = searchAttributes[property];
                switch (property.ToLower())
                {
                    case "name":
                        if (!product.Name.ToLower().Contains(((string)value).ToLower())) { result = false; }
                        break;
                    case "category":
                        if (!product.Category.ToLower().Equals(((string)value).ToLower())) { result = false; }
                        break;
                    case "lowprice":
                        if (product.Price < (Double)value) { result = false; }
                        break;
                    case "highprice":
                        if (product.Price > (Double)value) { result = false; }
                        break;
                    case "productrating":
                        if (product.Rating < (Double)value) { result = false; }
                        break;
                    case "storerating":
                        if (StoreRating < (Double)value) { result = false; }
                        break;
                    case "keywords":
                        bool found = false;
                        List<string> productKeywords = product.Keywords.Select(word => word.ToLower()).ToList();
                        //foreach (string keyword in (List<String>)value)
                        List<string> searchWords = (List<String>)value;
                        for (int i=0; i<searchWords.Count && !found ; i++)
                        {
                            if (productKeywords.Contains(searchWords[i].ToLower()))
                            {
                                //One keyword has been found
                                found=true;
                            }
                        }
                        //No keyword has been found
                        if (!found)
                        {
                            result = false;
                        }
                        break;
                }
            }
            return result;
        }

        public Result<Product> GetProduct(String productID)
        {
            if (Products.TryGetValue(productID, out Product product))
            {
                return new Result<Product>("", true, product);
            }
            //else failed
            return new Result<Product>($"Product (ID: {productID}) not found.\n", false, null);
        }
    }
}
