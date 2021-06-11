using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountComposition;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountConditions;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies
{
    public interface IPolicyManager
    {
        double GetTotalBagPrice(ConcurrentDictionary<Product, int> products, string discountCode = "");
        Result<bool> AdheresToPolicy(ConcurrentDictionary<Product, int> products, User user);
        Result<IDiscountPolicy> AddDiscountPolicy(Dictionary<string, object> info);
        Result<IDiscountPolicy> AddDiscountPolicy(Dictionary<string, object> info, String id);
        Result<IDiscountCondition> AddDiscountCondition(Dictionary<string, object> info, String id);
        Result<Boolean> AddDiscountPolicy(IDiscountPolicy policy);
        Result<Boolean> AddDiscountPolicy(IDiscountPolicy policy, String id);
        Result<Boolean> AddDiscountCondition(IDiscountCondition condition, String id);
        Result<IDiscountPolicy> RemoveDiscountPolicy(String id);
        Result<IDiscountCondition> RemoveDiscountCondition(String id);
        Result<Boolean> EditDiscountPolicy(Dictionary<string, object> info, String id);
        Result<Boolean> EditDiscountCondition(Dictionary<string, object> info, String id);
        Result<IDictionary<string, object>> GetDiscountPolicyData();
        Result<IDictionary<string, object>> GetPurchasePolicyData();
        Result<IPurchasePolicy> AddPurchasePolicy(Dictionary<string, object> info);
        Result<Boolean> AddPurchasePolicy(IPurchasePolicy policy);
        Result<IPurchasePolicy> AddPurchasePolicy(Dictionary<string, object> info, string id);
        Result<Boolean> AddPurchasePolicy(IPurchasePolicy policy, string id);
        Result<IPurchasePolicy> RemovePurchasePolicy(string id);
        Result<bool> EditPurchasePolicy(Dictionary<string, object> info, string id);
    }

    public class PolicyManager : IPolicyManager
    {

        public DiscountAddition MainDiscount { get; }
        public BuyNow MainPolicy { get; set; }
        public PolicyManager()
        {
            MainDiscount = new DiscountAddition();
            MainPolicy = new BuyNow();
        }
        
        public PolicyManager(DiscountAddition mainDiscount, BuyNow mainPolicy)
        {
            MainDiscount = mainDiscount;
            MainPolicy = mainPolicy;
        }

        public double GetTotalBagPrice(ConcurrentDictionary<Product, int> products, string discountCode = "")
        {
            Result<Dictionary<Product, Double>> discountsResult = MainDiscount.CalculateDiscount(products, discountCode);
            if (!discountsResult.ExecStatus)
                return -1;

            Dictionary<Product, Double> discounts = discountsResult.Data;
            Double price = 0;
            foreach (KeyValuePair<Product, int> entry in products)
            {
                if (discounts.ContainsKey(entry.Key))
                    price += entry.Key.Price * entry.Value * (100 - discounts[entry.Key]) / 100;
                else
                    price += entry.Key.Price * entry.Value;
            }

            return price;
        }
   
        public Result<bool> AdheresToPolicy(ConcurrentDictionary<Product, int> products, User user)
        {
            return MainPolicy.IsConditionMet(products, user);
        }

        public Result<IDiscountPolicy> AddDiscountPolicy(Dictionary<string, object> info)
        {
            Result<IDiscountPolicy> discountResult = CreateDiscount(info);
            if (discountResult.ExecStatus)
            {
                Result<bool> res = AddDiscountPolicy(discountResult.Data);
                if (res.ExecStatus)
                    return new Result<IDiscountPolicy>(res.Message, res.ExecStatus, discountResult.Data);
            }

            return new Result<IDiscountPolicy>(discountResult.Message, false, null);
        }

        public Result<IDiscountPolicy> AddDiscountPolicy(Dictionary<string, object> info, String id)
        {
            Result<IDiscountPolicy> discountResult = CreateDiscount(info);
            if (discountResult.ExecStatus)
            {
                Result<bool> res = AddDiscountPolicy(discountResult.Data, id);
                if (res.ExecStatus)
                    return new Result<IDiscountPolicy>(res.Message, res.ExecStatus, discountResult.Data);
            }
            return new Result<IDiscountPolicy>(discountResult.Message, false, null);
        }

        public Result<IDiscountCondition> AddDiscountCondition(Dictionary<string, object> info, String id)
        {
            Result<IDiscountCondition> discountConditionResult = CreateDiscountCondition(info);
            if (discountConditionResult.ExecStatus)
            {
                Result<bool> res = AddDiscountCondition(discountConditionResult.Data, id);
                if (res.ExecStatus)
                    return new Result<IDiscountCondition>(res.Message, res.ExecStatus, discountConditionResult.Data);
            } 
            return new Result<IDiscountCondition>(discountConditionResult.Message, false, null);
        }

        public Result<Boolean> AddDiscountPolicy(IDiscountPolicy discount)
        {
            Result<bool> result = MainDiscount.AddDiscount(MainDiscount.Id, discount);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The discount has been added successfully", true, true);
                else return new Result<bool>($"The discount could not be added", false, false);
            }
            return result;
        }

        public Result<Boolean> AddDiscountPolicy(IDiscountPolicy discount, String id)
        {
            Result<bool> result = MainDiscount.AddDiscount(id, discount);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The discount has been added successfully", true, true);
                else return new Result<bool>($"The discount addition failed because the discount with an id ${id} was not found", false, false);
            }
            return result;
        }

        public Result<Boolean> AddDiscountCondition(IDiscountCondition condition, String id)
        {
            Result<bool> result = MainDiscount.AddCondition(id, condition);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The discount condition has been added successfully", true, true);
                else return new Result<bool>($"The discount condition addition failed because the discount condition with an id ${id} was not found", false, false);
            }
            return result;
        }

        public Result<IDiscountPolicy> RemoveDiscountPolicy(string id)
        {
            Result<IDiscountPolicy> result = MainDiscount.RemoveDiscount(id);
            if (result.ExecStatus)
            {
                if (result.Data != null)
                    return new Result<IDiscountPolicy>("The discount has been removed successfully", true, result.Data);
                else return new Result<IDiscountPolicy>($"The discount removal failed because the discount with an id ${id} was not found", false, null);
            }
            return result;
        }

        public Result<IDiscountCondition> RemoveDiscountCondition(string id)
        {
            Result<IDiscountCondition> result = MainDiscount.RemoveCondition(id);
            if (result.ExecStatus)
            {
                if (result.Data != null)
                    return new Result<IDiscountCondition>("The discount condition has been removed successfully", true, result.Data);
                else return new Result<IDiscountCondition>($"The discount condition removal failed because the discount condition with an id ${id} was not found", false, null);
            }
            return result;
        }

        public Result<IDictionary<string, object>> GetDiscountPolicyData()
        {
            return MainDiscount.GetData();
        }

        public Result<IDictionary<string, object>> GetPurchasePolicyData()
        {
            return MainPolicy.GetData();
        }

        public Result<IPurchasePolicy> AddPurchasePolicy(Dictionary<string, object> info)
        {
            Result<IPurchasePolicy> result = CreatePurchasePolicy(info);
            if (!result.ExecStatus)
                return new Result<IPurchasePolicy>(result.Message, false, null);

            IPurchasePolicy policy = result.Data;
            Result<bool> res = AddPurchasePolicy(policy);
            if (res.ExecStatus)
            {
                return new Result<IPurchasePolicy>(res.Message, res.ExecStatus, policy);
            }
            return new Result<IPurchasePolicy>(res.Message, res.ExecStatus, null);
        }

        public Result<Boolean> AddPurchasePolicy(IPurchasePolicy policy)
        {
            Result<bool> result = MainPolicy.AddPolicy(policy, MainPolicy.Policy.Id);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The policy has been added successfully", true, true);
                else return new Result<bool>($"The policy could not be added", false, false);
            }
            return result;
        }

        public Result<IPurchasePolicy> AddPurchasePolicy(Dictionary<string, object> info, String id)
        {
            Result<IPurchasePolicy> result = CreatePurchasePolicy(info);
            if (!result.ExecStatus)
                return new Result<IPurchasePolicy>(result.Message, false, null);

            IPurchasePolicy policy = result.Data;
            Result<bool> res = AddPurchasePolicy(policy, id);
            if (res.ExecStatus)
            {
                return new Result<IPurchasePolicy>(res.Message, res.ExecStatus, policy);
            }
            return new Result<IPurchasePolicy>(res.Message, res.ExecStatus, null);
        }

        public Result<Boolean> AddPurchasePolicy(IPurchasePolicy policy, String id)
        {
            Result<bool> result = MainPolicy.AddPolicy(policy, id);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The policy has been added successfully", true, true);
                else return new Result<bool>($"The policy addition failed because the policy with an id ${id} was not found", false, false);
            }
            return result;
        }

        public Result<IPurchasePolicy> RemovePurchasePolicy(string id)
        {
            Result<IPurchasePolicy> result = MainPolicy.RemovePolicy(id);
            if (result.ExecStatus)
            {
                if (result.Data != null)
                    return new Result<IPurchasePolicy>("The policy has been removed successfully", true, result.Data);
                else return new Result<IPurchasePolicy>($"The policy removal failed because the policy with an id ${id} was not found", false, null);
            }
            return result;
        }

        private Result<IPurchasePolicy> CreatePurchasePolicy(Dictionary<string, object> info)
        {
            if (!info.ContainsKey("type"))
                return new Result<IPurchasePolicy>("Can't create a purchase Policy without a type", false, null);

            string type = ((JsonElement)info["type"]).GetString();
            switch (type)
            {
                case "AndPolicy":
                    return AndPolicy.create(info);
                case "OrPolicy":
                    return OrPolicy.create(info);
                case "ConditionalPolicy":
                    return ConditionalPolicy.create(info);
                case "MaxProductPolicy":
                    return MaxProductPolicy.create(info);
                case "MinProductPolicy":
                    return MinProductPolicy.create(info);
                case "MinAgePolicy":
                    return MinAgePolicy.create(info);
                case "RestrictedHoursPolicy":
                    return RestrictedHoursPolicy.create(info);

                default:
                    return new Result<IPurchasePolicy>("Can't recognise this purchase policy type: " + type, false, null);
            }
        }

        public Result<bool> EditPurchasePolicy(Dictionary<string, object> info, string id)
        {
            Result<bool> result = MainPolicy.EditPolicy(info, id);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The policy has been edited successfully", true, true);
                return new Result<bool>($"The Purchase policy edit failed because the policy with an id ${id} was not found", false, false);
            }
            return result;
        }

        private Result<IDiscountPolicy> CreateDiscount(Dictionary<string, object> info)
        {
            if (!info.ContainsKey("type"))
                return new Result<IDiscountPolicy>("Can't create a discount without a type", false, null);

            string type = ((JsonElement)info["type"]).GetString();
            switch (type)
            {
                case "VisibleDiscount":
                    return VisibleDiscount.create(info);
                case "DiscreetDiscount":
                    return DiscreetDiscount.create(info);
                case "ConditionalDiscount":
                    return ConditionalDiscount.create(info);
                case "DiscountAddition":
                    return DiscountAddition.create(info);
                case "DiscountAnd":
                    return DiscountAnd.create(info);
                case "DiscountMax":
                    return DiscountMax.create(info);
                case "DiscountMin":
                    return DiscountMin.create(info);
                case "DiscountOr":
                    return DiscountOr.create(info);
                case "DiscountXor":
                    return DiscountXor.create(info);

                default:
                    return new Result<IDiscountPolicy>("Can't recognise this discount type: " + type, false, null);
            }
        }

        private Result<IDiscountCondition> CreateDiscountCondition(Dictionary<string, object> info)
        {
            if (!info.ContainsKey("type"))
                return new Result<IDiscountCondition>("Can't create a condition without a type", false, null);

            string type = ((JsonElement)info["type"]).GetString();
            switch (type)
            {
                case "DiscountConditionAnd":
                    return DiscountConditionAnd.create(info);
                case "DiscountConditionOr":
                    return DiscountConditionOr.create(info);
                case "MaxProductCondition":
                    return MaxProductCondition.create(info);
                case "MinProductCondition":
                    return MinProductCondition.create(info);
                case "MinBagPriceCondition":
                    return MinBagPriceCondition.create(info);


                default:
                    return new Result<IDiscountCondition>("Can't recognise this condition type: " + type, false, null);
            }
        }

        public Result<bool> EditDiscountPolicy(Dictionary<string, object> info, string id)
        {
            Result<bool> result = MainDiscount.EditDiscount(info, id);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The discount has been edited successfully", true, true);
                else return new Result<bool>($"The discount edit failed because the discount with an id ${id} was not found", false, false);
            }
            return result;
        }

        public Result<bool> EditDiscountCondition(Dictionary<string, object> info, string id)
        {
            Result<bool> result = MainDiscount.EditCondition(info, id);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The discount condition has been edited successfully", true, true);
                else return new Result<bool>($"The discount condition edit failed because the discount condition with an id ${id} was not found", false, false);
            }
            return result;
        }

        
    }

/*    // Update Store in DB
    var filter = Builders<BsonDocument>.Filter.Eq("_id", store.Id);
    var update = Builders<BsonDocument>.Update.Set("isClosed", false);
    mapper.UpdateStore(filter, update);
*/
}
