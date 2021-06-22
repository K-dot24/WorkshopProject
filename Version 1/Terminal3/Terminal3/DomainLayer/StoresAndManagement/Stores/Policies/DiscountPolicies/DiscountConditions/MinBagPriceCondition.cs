﻿using MongoDB.Bson;
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
    public class MinBagPriceCondition : AbstractDiscountCondition
    {

        public Double MinPrice { set; get; }

        public MinBagPriceCondition(Double minPrice, String id = "") : base(new Dictionary<string, object>(), id)
        {
            MinPrice = minPrice;
        }

        public static Result<IDiscountCondition> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create MinBagPriceCondition: ";
            if (!info.ContainsKey("MinPrice"))
                return new Result<IDiscountCondition>(errorMsg + "MinPrice not found", false, null);
            Double minPrice = ((JsonElement)info["MinPrice"]).GetDouble();

            return new Result<IDiscountCondition>("Succesfuly created discount condition Min bag price", true, new MinBagPriceCondition(minPrice));
        }

        public override Result<bool> isConditionMet(ConcurrentDictionary<Product, int> products)
        {
            double priceAcc = 0;
            foreach(KeyValuePair<Product, int> entry in products)
            {
                priceAcc += entry.Key.Price * entry.Value;
                if (priceAcc >= MinPrice)
                    return new Result<bool>("", true, true);
            }
            return new Result<bool>("", true, false);
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
                {"MinPrice", MinPrice} 
            };*/
            IDictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", Id },
                { "name", "Bag price >= " + MinPrice},
                { "children", new Dictionary<String, object>[0] }
            };
            return new Result<IDictionary<string, object>>("", true, dict);
            return new Result<IDictionary<string, object>>("", true, dict);
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            if (Id != id)
                return new Result<bool>("", true, false);

            if (info.ContainsKey("MinPrice"))
            {
                MinPrice = ((JsonElement)info["MinPrice"]).GetDouble();
                var filter = Builders<BsonDocument>.Filter.Eq("_id", Id);
                var update_discount = Builders<BsonDocument>.Update.Set("MinPrice", MinPrice);
                Mapper.getInstance().UpdateMinBagPriceCondition(filter, update_discount);
            }

            return new Result<bool>("", true, true);
        }
    }
}
