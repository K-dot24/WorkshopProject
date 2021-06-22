﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DataAccessLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountConditionsData;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class ConditionalDiscount : AbstractDiscountPolicy
    {

        public IDiscountCondition Condition { set; get; }
        public IDiscountPolicy Discount { set; get; }

        public ConditionalDiscount(IDiscountPolicy discount, IDiscountCondition condition, String id = "") : base(new Dictionary<string, object>(), id)
        {
            Condition = condition;
            Discount = discount;
        }

        public static Result<IDiscountPolicy> create(Dictionary<string, object> info)
        {
            return new Result<IDiscountPolicy>("", true, new ConditionalDiscount(null, null));
        }

        public override Result<Dictionary<Product, Double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            if(Condition == null || Discount == null)
                return new Result<Dictionary<Product, Double>>("", true, new Dictionary<Product, Double>());
            
            Result<bool> isEligible = Condition.isConditionMet(products);
            if (isEligible.ExecStatus && isEligible.Data)
            {
                return Discount.CalculateDiscount(products, code);
            }
            return new Result<Dictionary<Product, Double>>("", true, new Dictionary<Product, Double>());
        }

        public override Result<bool> AddDiscount(String id, IDiscountPolicy discount)
        {
            if (Id == id)
            {
                Discount = discount;
                var update_discount = Builders<BsonDocument>.Update.Set("Discount", discount.Id);
                Mapper.getInstance().UpdatePolicy(this, update_discount);
                return new Result<bool>("", true, true);
            }
            else if(Discount != null)
                return Discount.AddDiscount(id, discount);
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountPolicy> RemoveDiscount(String id)
        {
            if (Discount.Id.Equals(id))
            {
                IDiscountPolicy oldDiscount = Discount;
                Discount = null;
                var update_discount = Builders<BsonDocument>.Update.Set("Discount", "");
                Mapper.getInstance().UpdatePolicy(this, update_discount);
                return new Result<IDiscountPolicy>("", true, oldDiscount);
            }
            else if (Discount != null)
                return Discount.RemoveDiscount(id);
            return new Result<IDiscountPolicy>("", true, null);
        }

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            if (Id == id)
            {
                Condition = condition;
                var update_discount = Builders<BsonDocument>.Update.Set("Condition", condition.Id);
                Mapper.getInstance().UpdatePolicy(this, update_discount);
                return new Result<bool>("", true, true);
            }
            else if (Condition != null)
                return Condition.AddCondition(id, condition);
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountCondition> RemoveCondition(string id)
        {
            if (Condition == null)
                return new Result<IDiscountCondition>("", true, null);
            if (Condition.Id.Equals(id))
            {
                IDiscountCondition oldCondition = Condition;
                Condition = null;
                var update_discount = Builders<BsonDocument>.Update.Set("Condition", "");
                Mapper.getInstance().UpdatePolicy(this, update_discount);
                return new Result<IDiscountCondition>("", true, oldCondition);
            }
            return Condition.RemoveCondition(id);
        }

        public override Result<IDictionary<string, object>> GetData()
        {
            /*IDictionary<string, object> dict = new Dictionary<string, object>() {
                { "type", "VisibleDiscount" },
                { "Id", Id },               
                { "Condition", null },
                { "Discount", null }
            };*/
            IDictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", Id },
                { "name", "Conditional"},
                { "children", new Dictionary<String, object>[0] }
            };
            List<IDictionary<String, object>> children = new List<IDictionary<string, object>>();
            if (Condition != null)
            {
                Result<IDictionary<string, object>> conditionDataResult = Condition.GetData();
                if (!conditionDataResult.ExecStatus)
                    return conditionDataResult;
                children.Add(conditionDataResult.Data);
            }

            if (Discount!= null)
            {
                Result<IDictionary<string, object>> discountDataResult = Discount.GetData();
                if (!discountDataResult.ExecStatus)
                    return discountDataResult;
                children.Add(discountDataResult.Data);
            }
            dict["children"] = children.ToArray();

            return new Result<IDictionary<string, object>>("", true, dict);
        }

        public override Result<bool> EditDiscount(Dictionary<string, object> info, string id)
        {
            if (Id != id)
            {
                if (Discount != null)
                    return Discount.EditDiscount(info, id);
                return new Result<bool>("", true, false);
            }

            return new Result<bool>("", true, true);
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            if (Discount != null)
                return Discount.EditCondition(info, id);
            return new Result<bool>("", true, false);
        }
    }
}
