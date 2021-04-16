using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{

    public interface IInventoryManager
    {
    }

    public class InventoryManager : IInventoryManager
    {
        //Properties
        public ConcurrentDictionary<String, Product> Products { get; private set; }

        //Constructor
        public InventoryManager()
        {
            this.Products = new ConcurrentDictionary<String, Product>();
        }

        //TODO
        internal Result<List<Product>> SearchProduct(IDictionary<String, Object> productDetails)
        {
            List<Product> searchResults = new List<Product>();
            
        }
    }
}
