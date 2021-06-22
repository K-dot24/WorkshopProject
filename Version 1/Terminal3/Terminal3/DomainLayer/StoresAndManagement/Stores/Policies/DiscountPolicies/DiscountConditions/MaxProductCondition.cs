using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Terminal3.DataAccessLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions
{
    public class MaxProductCondition : AbstractDiscountCondition
    {

        public int MaxQuantity { set; get; }
        public String ProductId { set; get; }

        public MaxProductCondition(String productId, int maxQuantity, String id = "") : base(new Dictionary<string, object>(), id)
        {
            ProductId = productId;
            MaxQuantity = maxQuantity;
        }

        public static Result<IDiscountCondition> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create MaxProductCondition: ";
            if (!info.ContainsKey("MaxQuantity"))
                return new Result<IDiscountCondition>(errorMsg + "MaxQuantity not found", false, null);
            int maxQuantity = ((JsonElement)info["MaxQuantity"]).GetInt32();

            if (!info.ContainsKey("ProductId"))
                return new Result<IDiscountCondition>("ProductId not found", false, null);
            String productId = ((JsonElement)info["ProductId"]).GetString();

            return new Result<IDiscountCondition>("Succesfuly created discount condition Max", true, new MaxProductCondition(productId, maxQuantity));
        }

        public override Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
        {
            Product myProduct = ContainsProduct(products);
            if(myProduct == null)
                return new Result<bool>("", true, true);

            return new Result<bool>("", true, products[myProduct] <= MaxQuantity);
        }

        private Product ContainsProduct(ConcurrentDictionary<Product, int> products)
        {
            foreach (KeyValuePair<Product, int> entry in products)
            {
                if (entry.Key.Id.Equals(ProductId))
                    return entry.Key;
            }
            return null;
        }

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountCondition> RemoveCondition(string id)
        {
            return new Result<IDiscountCondition>("", true, null);
        }

        public override Result<IDictionary<string, object>> GetData()
        {
            /*IDictionary<string, object> dict = new Dictionary<string, object>() {
                {"type", "DiscountConditionAnd" },
                {"Id", Id },
                {"MaxQuantity", MaxQuantity},
                {"ProductId", ProductId }
            };*/
            IDictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", Id },
                { "name", "" + ProductId + " <= " + MaxQuantity},
                { "children", new Dictionary<String, object>[0] }
            };
            return new Result<IDictionary<string, object>>("", true, dict);
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            if (Id != id)
                return new Result<bool>("", true, false);

            if (info.ContainsKey("MaxQuantity"))
            {
                MaxQuantity = ((JsonElement)info["MaxQuantity"]).GetInt32();
                var filter = Builders<BsonDocument>.Filter.Eq("_id", Id);
                var update_discount = Builders<BsonDocument>.Update.Set("MaxQuantity", MaxQuantity);
                Mapper.getInstance().UpdateMaxProductCondition(filter, update_discount);
            }

            if (info.ContainsKey("ProductId"))
            {
                ProductId = ((JsonElement)info["ProductId"]).GetString();
                var filter = Builders<BsonDocument>.Filter.Eq("_id", Id);
                var update_discount = Builders<BsonDocument>.Update.Set("ProductId", ProductId);
                Mapper.getInstance().UpdateMaxProductCondition(filter, update_discount);
            }

            return new Result<bool>("", true, true);
        }
    }
}
