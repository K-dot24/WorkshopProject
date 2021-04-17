using Xunit;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Tests
{
    public class InventoryManagerTests
    {
        //Properties
        public InventoryManager InventoryManager { get; }

        //Constructor
        public InventoryManagerTests()
        {
            InventoryManager = new InventoryManager();
        }

        [Fact()]
        [Trait("Category","Unit")]
        public void SearchProductTestBySubstring()
        {
            string[] keywords = { "Healty", "Sport", "Market" };
            Product product = new Product("Banana", 19.9, 10, "Fruit",new LinkedList<String>(keywords));
            InventoryManager.Products.TryAdd(product.Id,product);
            /*ProductSearchAttributes searchAttributes = ObjectDictionaryMapper<ProductSearchAttributes>.GetObject(new Dictionary<String, Object>() 
                                                        {{ "Name", "nana" }});
            */
            IDictionary<String,Object> searchAttributes =  new Dictionary<String, Object>()
                                                            {{ "Name", "nana" }};
            Result<List<Product>> result = InventoryManager.SearchProduct(0, searchAttributes);
            Assert.True(result.ExecStatus);
            Assert.Single(result.Data);
            Assert.Equal("Banana", result.Data[0].Name);

        }
    }
}