using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DataAccessLayer;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountComposition;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscountOr : AbstractDiscountPolicy
    {

        public List<IDiscountPolicy> Discounts { get; }

        public DiscountOr(String id = "") : base(new Dictionary<string, object>(), id)
        {
            Discounts = new List<IDiscountPolicy>();
        }

        public static Result<IDiscountPolicy> create(Dictionary<string, object> info)
        {
            return new Result<IDiscountPolicy>("", true, new DiscountOr());
        }

        public DiscountOr(List<IDiscountPolicy> discounts, String id = "") : base(new Dictionary<string, object>(), id)
        {
            Discounts = discounts;
            if (Discounts == null)
                Discounts = new List<IDiscountPolicy>();
        }

        public override Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            List<Dictionary<Product, double>> results = new List<Dictionary<Product, double>>();

            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<Dictionary<Product, double>> result = myDiscount.CalculateDiscount(products, code);

                if (result.Data == null)
                    result.Data=new Dictionary<Product, double>();

                results.Add(result.Data);
            }

            return CombineOrDiscounts(results);
        }

        private Result<Dictionary<Product, double>> CombineOrDiscounts(List<Dictionary<Product, double>> discounts)
        {
            if (discounts == null)
                return new Result<Dictionary<Product, double>>("", true, new Dictionary<Product, double>());
            if (discounts.Count == 0)
                return new Result<Dictionary<Product, double>>("", true, new Dictionary<Product, double>());

            Dictionary<Product, double> acc = discounts[0];

            foreach (Dictionary<Product, double> discount in discounts)
            {
                foreach (KeyValuePair<Product, double> entry in discount)
                {
                    if (!acc.ContainsKey(entry.Key))
                        acc[entry.Key] = entry.Value;
                    else if (acc[entry.Key] < entry.Value)
                        acc[entry.Key] = entry.Value;
                }
            }

            return new Result<Dictionary<Product, double>>("", true, acc);
        }
        public override Result<bool> AddDiscount(String id, IDiscountPolicy discount)
        {
            if (Id.Equals(id))
            {
                Discounts.Add(discount);
                var update_discount = Builders<BsonDocument>.Update.Set("Discounts", ConvertDiscountToIDs());
                Mapper.getInstance().UpdatePolicy(this, update_discount);
                return new Result<bool>("", true, true);
            }
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<bool> result = myDiscount.AddDiscount(id, discount);
                if (result.ExecStatus && result.Data)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountPolicy> RemoveDiscount(String id)
        {
            IDiscountPolicy toBeRemoved = getDiscount(id);
            if (toBeRemoved != null)
            {
                Discounts.Remove(toBeRemoved);
                var update_discount = Builders<BsonDocument>.Update.Set("Discounts", ConvertDiscountToIDs());
                Mapper.getInstance().UpdatePolicy(this, update_discount);
                return new Result<IDiscountPolicy>("", true, toBeRemoved);
            }
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<IDiscountPolicy> result = myDiscount.RemoveDiscount(id);
                if (result.ExecStatus && result.Data != null)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<IDiscountPolicy>("", true, null);
        }

        private IDiscountPolicy getDiscount(String id)
        {
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                if (myDiscount.Id.Equals(id))
                    return myDiscount;
            }
            return null;
        }

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<bool> result = myDiscount.AddCondition(id, condition);
                if (result.ExecStatus && result.Data)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountCondition> RemoveCondition(string id)
        {
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<IDiscountCondition> result = myDiscount.RemoveCondition(id);
                if (result.ExecStatus && result.Data != null)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<IDiscountCondition>("", true, null);
        }

        public override Result<IDictionary<string, object>> GetData()
        {
            /*IDictionary<string, object> dict = new Dictionary<string, object>() { 
                {"type", "DiscountAddition" },
                {"Id", Id },
                {"Discounts", null }
            };*/
            IDictionary<string, object> dict = new Dictionary<string, object>() {
                { "id", Id },
                { "name", "Or Discount"},
                { "children", new Dictionary<String, object>[0] }
            };
            List<IDictionary<string, object>> children = new List<IDictionary<string, object>>();
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<IDictionary<string, object>> discountResult = myDiscount.GetData();
                if (!discountResult.ExecStatus)
                    return discountResult;
                children.Add(discountResult.Data);
            }
            dict["children"] = children.ToArray();
            return new Result<IDictionary<string, object>>("", true, dict);
        }

        public override Result<bool> EditDiscount(Dictionary<string, object> info, string id)
        {
            if (Id != id)
            {
                foreach (IDiscountPolicy myDiscount in Discounts)
                {
                    Result<bool> result = myDiscount.EditCondition(info, id);
                    if (result.ExecStatus && result.Data)
                        return result;
                    if (!result.ExecStatus)
                        return result;
                }
                return new Result<bool>("", true, false);
            }

            return new Result<bool>("", true, true);
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            foreach (IDiscountPolicy myDiscount in Discounts)
            {
                Result<bool> result = myDiscount.EditCondition(info, id);
                if (result.ExecStatus && result.Data)
                    return result;
                if (!result.ExecStatus)
                    return result;
            }
            return new Result<bool>("", true, false);
        }

        private ConcurrentDictionary<String, String> ConvertDiscountToIDs()
        {
            ConcurrentDictionary<String, String> list = new ConcurrentDictionary<String, String>();    //<id , type>

            foreach (var discount in Discounts)
            {
                string[] type = discount.GetType().ToString().Split('.');
                string discount_type = type[type.Length - 1];
                list.TryAdd(discount.Id, discount_type);
            }

            return list;
        }
    }
}
