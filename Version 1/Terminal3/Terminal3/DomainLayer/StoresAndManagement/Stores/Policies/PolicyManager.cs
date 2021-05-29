using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        Result<Boolean> AddDiscountPolicy(Dictionary<string, object> info);
        Result<Boolean> AddDiscountPolicy(Dictionary<string, object> info, String id);
        Result<Boolean> AddDiscountCondition(Dictionary<string, object> info, String id);
        Result<Boolean> AddDiscountPolicy(IDiscountPolicy policy);
        Result<Boolean> AddDiscountPolicy(IDiscountPolicy policy, String id);
        Result<Boolean> AddDiscountCondition(IDiscountCondition condition, String id);
        Result<Boolean> RemoveDiscountPolicy(String id);
        Result<Boolean> RemoveDiscountCondition(String id);
        Result<Boolean> EditDiscountPolicy(Dictionary<string, object> info, String id);
        Result<Boolean> EditDiscountCondition(Dictionary<string, object> info, String id);
        Result<IDiscountPolicyData> GetDiscountPolicyData();
        Result<IPurchasePolicyData> GetPurchasePolicyData();
        Result<Boolean> AddPurchasePolicy(Dictionary<string, object> info);
        Result<Boolean> AddPurchasePolicy(IPurchasePolicy policy);
        Result<Boolean> AddPurchasePolicy(Dictionary<string, object> info, string id);
        Result<Boolean> AddPurchasePolicy(IPurchasePolicy policy, string id);
        Result<Boolean> RemovePurchasePolicy(string id);
        Result<bool> EditPurchasePolicy(Dictionary<string, object> info, string id);
        Result<bool> EditPurchasePolicy(IPurchasePolicy policy, string id);
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

        public Result<Boolean> AddDiscountPolicy(Dictionary<string, object> info)
        {
            Result<IDiscountPolicy> discountResult = CreateDiscount(info);
            if (discountResult.ExecStatus)
                return AddDiscountPolicy(discountResult.Data);
            return new Result<bool>(discountResult.Message, false, false);
        }

        public Result<Boolean> AddDiscountPolicy(Dictionary<string, object> info, String id)
        {
            Result<IDiscountPolicy> discountResult = CreateDiscount(info);
            if (discountResult.ExecStatus)
                return AddDiscountPolicy(discountResult.Data, id);
            return new Result<bool>(discountResult.Message, false, false);
        }

        public Result<Boolean> AddDiscountCondition(Dictionary<string, object> info, String id)
        {
            Result<IDiscountCondition> discountConditionResult = CreateDiscountCondition(info);
            if (discountConditionResult.ExecStatus)
                return AddDiscountCondition(discountConditionResult.Data, id);
            return new Result<bool>(discountConditionResult.Message, false, false);
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

        public Result<bool> RemoveDiscountPolicy(string id)
        {
            Result<bool> result = MainDiscount.RemoveDiscount(id);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The discount has been removed successfully", true, true);
                else return new Result<bool>($"The discount removal failed because the discount with an id ${id} was not found", false, false);
            }
            return result;
        }

        public Result<bool> RemoveDiscountCondition(string id)
        {
            Result<bool> result = MainDiscount.RemoveCondition(id);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The discount condition has been removed successfully", true, true);
                else return new Result<bool>($"The discount condition removal failed because the discount condition with an id ${id} was not found", false, false);
            }
            return result;
        }

        public Result<IDiscountPolicyData> GetDiscountPolicyData()
        {
            return MainDiscount.GetData();
        }

        public Result<IPurchasePolicyData> GetPurchasePolicyData()
        {
            return MainPolicy.GetData();
        }

        public Result<Boolean> AddPurchasePolicy(Dictionary<string, object> info)
        {
            Result<IPurchasePolicy> result = CreatePurchasePolicy(info);
            if (!result.ExecStatus)
                return new Result<bool>(result.Message, false, false);
            IPurchasePolicy policy = result.Data;
            return AddPurchasePolicy(policy);
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

        public Result<Boolean> AddPurchasePolicy(Dictionary<string, object> info, String id)
        {
            Result<IPurchasePolicy> result = CreatePurchasePolicy(info);
            if (!result.ExecStatus)
                return new Result<bool>(result.Message, false, false);
            IPurchasePolicy policy = result.Data;
            return AddPurchasePolicy(policy, id);
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

        public Result<bool> RemovePurchasePolicy(string id)
        {
            Result<bool> result = MainPolicy.RemovePolicy(id);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The policy has been removed successfully", true, true);
                else return new Result<bool>($"The policy removal failed because the policy with an id ${id} was not found", false, false);
            }
            return result;
        }

        private Result<IPurchasePolicy> CreatePurchasePolicy(Dictionary<string, object> info)
        {
            if (!info.ContainsKey("type"))
                return new Result<IPurchasePolicy>("Can't create a discount without a type", false, null);

            string type = (string)info["type"];
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
                    return new Result<IPurchasePolicy>("Can't recognise this discount type: " + type, false, null);
            }
        }

        public Result<bool> EditPurchasePolicy(Dictionary<string, object> info, string id)
        {
            Result<IPurchasePolicy> result = CreatePurchasePolicy(info);
            if (!result.ExecStatus)
                return new Result<bool>(result.Message, false, false);
            IPurchasePolicy policy = result.Data;
            return EditPurchasePolicy(policy, id);
        }

        public Result<bool> EditPurchasePolicy(IPurchasePolicy policy, string id)
        {
            Result<bool> result = MainPolicy.EditPolicy(policy, id);
            if (result.ExecStatus)
            {
                if (result.Data)
                    return new Result<bool>("The policy has been edited successfully", true, true);
                else return new Result<bool>(result.Message, false, false);
            }
            return result;
        }

        private Result<IDiscountPolicy> CreateDiscount(Dictionary<string, object> info)
        {
            if (!info.ContainsKey("type"))
                return new Result<IDiscountPolicy>("Can't create a discount without a type", false, null);

            string type = (string)info["type"];
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

            string type = (string)info["type"];
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
}
