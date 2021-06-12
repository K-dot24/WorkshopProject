using Xunit;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using System.Linq;

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

        [Theory(Skip ="Not relevent")]
        [Trait("Category", "Unit")]
        [InlineData("Name", "nana", true)]
        [InlineData("name", "lala", false)]
        [InlineData("lowprice", "19.0", true)]
        [InlineData("lowprice", "25.0", false)]
        [InlineData("highprice", "25.0", true)]
        [InlineData("highprice", "12.0", false)]
        [InlineData("productrating", "4.0", true)]
        [InlineData("productrating", "5.0", false)]
        [InlineData("storerating", "3.2", true)]
        [InlineData("storerating", "5.0", false)]
        public void SearchProductTestByAttribute(string attribute, object value,Boolean shouldFound)
        {
            string[] keywords = { "Healty", "Sport", "Market" };
            Double productRating = 4.1;
            Double storeRating = 3.5;
            Product product = new Product("Banana", 19.9, 10, "Fruit",new LinkedList<String>(keywords));
            product.Rating = productRating;
            InventoryManager.Products.TryAdd(product.Id,product);
            IDictionary<String,Object> searchAttributes =  new Dictionary<String, Object>()
                                                            {{ attribute, value }};

            Result<List<Product>> result = InventoryManager.SearchProduct(storeRating, searchAttributes);
            Assert.Equal(shouldFound,result.ExecStatus);
            if (shouldFound) { 
                Assert.Single(result.Data);
                Assert.Equal(product.Name, result.Data[0].Name);
            }

        }
        [Theory(Skip = "Not relevent")]
        [Trait("Category", "Unit")]
        [InlineData("healty", true)]
        [InlineData("sport", true)]
        [InlineData("market", true)]
        [InlineData("food", false)]
        public void SearchProductTestByAttributeKeyWords( string keyword, Boolean shouldFound)
        {
            string[] keywords = { "Healty", "Sport", "Market" };
            List<string> keywordsForsearch = new List<string>() {keyword};
            Double productRating = 4.1;
            Double storeRating = 3.5;
            Product product = new Product("Banana", 19.9, 10, "Fruit", Keywords: new LinkedList<String>(keywords));
            product.Rating = productRating;
            InventoryManager.Products.TryAdd(product.Id, product);
            IDictionary<String, Object> searchAttributes = new Dictionary<String, Object>()
                                                            {{ "keywords", keywordsForsearch }};
            Result<List<Product>> result = InventoryManager.SearchProduct(storeRating, searchAttributes);
            Assert.Equal(shouldFound, result.ExecStatus);
            if (shouldFound)
            {
                Assert.Single(result.Data);
                Assert.Equal(product.Name, result.Data[0].Name);
            }

        }

        private static void RecurseDeserialize(Dictionary<string, object> result)
        {
            //Iterate throgh key/value pairs
            foreach (var keyValuePair in result.ToArray())
            {
                //Check to see if Newtonsoft thinks this is a JArray
                var jarray = keyValuePair.Value as JArray;

                if (jarray != null)
                {
                    //We have a JArray

                    //Convert JArray back to json and deserialize to a list of dictionaries
                    var dictionaries = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jarray.ToString());

                    //Set the result as the dictionary
                    result[keyValuePair.Key] = dictionaries;

                    //Iterate throught the dictionaries
                    foreach (var dictionary in dictionaries)
                    {
                        //Recurse
                        RecurseDeserialize(dictionary);
                    }
                }
            }
        }
    }

}