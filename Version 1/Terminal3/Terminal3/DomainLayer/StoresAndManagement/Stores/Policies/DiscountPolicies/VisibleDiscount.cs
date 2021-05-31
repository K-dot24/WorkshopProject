using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountTargets;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class VisibleDiscount : AbstractDiscountPolicy
    {

        public DateTime ExpirationDate { get; set; }
        public IDiscountTarget Target { get; set; }
        public Double Percentage { get; set; }

        public VisibleDiscount(DateTime expirationDate, IDiscountTarget target, Double percentage, String id="") : base(new Dictionary<string, object>(), id)
        {
            ExpirationDate = expirationDate;
            Target = target;
            if (percentage > 100)
                Percentage = 100;
            else if (percentage < 0)
                Percentage = 0;
            else
                Percentage = percentage;
        }

        public static Result<IDiscountPolicy> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create VisibleDiscount: ";
            if (!info.ContainsKey("ExpirationDate"))
                return new Result<IDiscountPolicy>(errorMsg + "ExpirationDate not found", false, null);
            DateTime expirationDate = createDateTime((JsonElement)info["ExpirationDate"]);

            if (!info.ContainsKey("Percentage"))
                return new Result<IDiscountPolicy>(errorMsg + "Percentage not found", false, null);
            Double percentage = ((JsonElement)info["Percentage"]).GetDouble();

            if (!info.ContainsKey("Target"))
                return new Result<IDiscountPolicy>(errorMsg + "Target not found", false, null);

            Result<IDiscountTarget> targetResult = createTarget((JsonElement)info["Target"]);
            if (!targetResult.ExecStatus)
                return new Result<IDiscountPolicy>(targetResult.Message, false, null);

            return new Result<IDiscountPolicy>("", true, new VisibleDiscount(expirationDate, targetResult.Data, percentage));
        }

        private static DateTime createDateTime(JsonElement timeElement)
        {
            String timeString = timeElement.GetString();
            DateTime time = DateTime.ParseExact(timeString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            return time;
        }

        private static Result<IDiscountTarget> createTarget(JsonElement targetElement)
        {
            Dictionary<string, object> targetDict = JsonSerializer.Deserialize<Dictionary<string, object>>(targetElement.GetRawText());

            return createTarget(targetDict);
        }

        public override Result<Dictionary<Product, Double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            //if the discount is expired
            if (DateTime.Now.CompareTo(ExpirationDate) >= 0)
                return new Result<Dictionary<Product, Double>>("", true, new Dictionary<Product, Double>());

            List<Product> targetProducts = Target.getTargets(products);
            Dictionary<Product, Double> resultDictionary = new Dictionary<Product, Double>();
            foreach(Product product in targetProducts)
            {
                resultDictionary.Add(product, Percentage);
            }

            return new Result<Dictionary<Product, double>>("", true, resultDictionary);
        }

        public override Result<bool> AddDiscount(String id, IDiscountPolicy discount)
        {
            if (Id.Equals(id))
                return new Result<bool>("Can't add a discount to a visible discount with an id " + id, false, false);
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountPolicy> RemoveDiscount(String id)
        {
            return new Result<IDiscountPolicy>("", true, null);
        }

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountCondition> RemoveCondition(string id)
        {
            return new Result<IDiscountCondition>("", true, null);
        }

        public override Result<IDiscountPolicyData> GetData()
        {
            IDiscountTargetData targetData = null;
            if (Target != null)
            {
                Result<IDiscountTargetData> targetDataResult = Target.GetData();
                if (!targetDataResult.ExecStatus)
                    return new Result<IDiscountPolicyData>(targetDataResult.Message, false, null);
                targetData = targetDataResult.Data;
            }

            return new Result<IDiscountPolicyData>("", true, new VisibleDiscountData(ExpirationDate, targetData, Percentage, Id));
        }

        private static Result<IDiscountTarget> createTarget(Dictionary<string, object> info)
        {
            if (!info.ContainsKey("type"))
                return new Result<IDiscountTarget>("Can't create a target without a type", false, null);

            string type = ((JsonElement)info["type"]).ToString();
            switch (type)
            {
                case "DiscountTargetShop":
                    return DiscountTargetShop.create(info);
                case "DiscountTargetCategories":
                    return DiscountTargetCategories.create(info);
                case "DiscountTargetProducts":
                    return DiscountTargetProducts.create(info);
                default:
                    return new Result<IDiscountTarget>("Can't recognise this target type: " + type, false, null);
            }
        }

        public override Result<bool> EditDiscount(Dictionary<string, object> info, string id)
        {
            if (Id != id)
                return new Result<bool>("", true, false);

            if (info.ContainsKey("ExpirationDate"))
                //ExpirationDate = (DateTime)info["ExpirationDate"];
                ExpirationDate = createDateTime((JsonElement)info["ExpirationDate"]);

            if (info.ContainsKey("Percentage"))
                Percentage = ((JsonElement)info["Percentage"]).GetDouble();

            if (info.ContainsKey("Target"))
            {
                Result<IDiscountTarget> targetResult = createTarget((JsonElement)info["Target"]);
                if (!targetResult.ExecStatus)
                    return new Result<bool>(targetResult.Message, false, false);

            }
            return new Result<bool>("", true, true);
        }

        public override Result<bool> EditCondition(Dictionary<string, object> info, string id)
        {
            return new Result<bool>("", true, false);
        }
    }
}
