using Xunit;
using Terminal3.DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

namespace Terminal3.DomainLayer.Tests
{
    public class ObjectDictionaryMapperTests
    {

        [Fact()]
        [Trait("Category", "Unit")]
        public void GetObjectTestTrue()
        {
            Dictionary<String, Object> dict = new Dictionary<string, object>()
                                        { {"Name","tes" },
                                          {"Category","sport"},
                                          {"LowPrice", 100 }
                                        };
            ProductSearchAttributes searchAttributesActual = ObjectDictionaryMapper<ProductSearchAttributes>.GetObject(dict);
            ProductSearchAttributes searchAttributesExpected = new ProductSearchAttributes(Name: "tes", Category: "sport", LowPrice: 100);
            Assert.True(searchAttributesActual.Equals(searchAttributesExpected));
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void GetObjectTestFalse()
        {
            Dictionary<String, Object> dict = new Dictionary<string, object>()
                                        { {"Name","tes" },
                                          {"Category","sport"},
                                        };
            ProductSearchAttributes searchAttributesActual = ObjectDictionaryMapper<ProductSearchAttributes>.GetObject(dict);
            ProductSearchAttributes searchAttributesExpected = new ProductSearchAttributes(Name: "tes", Category: "sport", LowPrice: 100);
            Assert.False(searchAttributesActual.Equals(searchAttributesExpected));
        }

        [Fact()]
        [Trait("Category", "Unit")]
        public void GetDictionaryTestTrue()
        {
            string[] keywords = { "Food", "Market" };
            Product product = new Product("Banana", 19.9, 10, "Fruit", new LinkedList<string>(keywords));
            IDictionary<String, Object> objectDict = ObjectDictionaryMapper<Product>.GetDictionary(product);
            Assert.True(objectDict["Name"].Equals("Banana"));
            Assert.True(objectDict["Price"].Equals(19.9));
            Assert.True(objectDict["Category"].Equals("Fruit"));

        }

    }
}