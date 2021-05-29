using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class RestrictedHoursPolicy : IPurchasePolicy
    {
        public TimeSpan StartRestrict { get; set; }
        public TimeSpan EndRestrict { get; set; }
        public string ProductId { get; set; }
        public string Id { get; }

        public RestrictedHoursPolicy(TimeSpan startRestrict, TimeSpan endRestrict, string productId, string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.StartRestrict = startRestrict;
            this.EndRestrict= endRestrict;
            this.ProductId = productId;
        }

        public static Result<IPurchasePolicy> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create RestrictedHoursPolicy: ";
            if (!info.ContainsKey("StartRestrict"))
                return new Result<IPurchasePolicy>(errorMsg + "StartRestrict not found", false, null);
            TimeSpan startRestrict= (TimeSpan)info["StartRestrict"];

            if (!info.ContainsKey("EndRestrict"))
                return new Result<IPurchasePolicy>(errorMsg + "EndRestrict not found", false, null);
            TimeSpan endRestrict = (TimeSpan)info["EndRestrict"];

            if (!info.ContainsKey("ProductId"))
                return new Result<IPurchasePolicy>(errorMsg + "ProductId not found", false, null);
            string productId = (string)info["ProductId"];

            return new Result<IPurchasePolicy>("", true, new RestrictedHoursPolicy(startRestrict, endRestrict, productId));
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            return new Result<bool>("", true, now > EndRestrict && now < StartRestrict);
        }

        public Result<bool> AddPolicy(IPurchasePolicy policy, string id)
        {
            if (this.Id.Equals(id))
                return new Result<bool>("Cannot add a policy to this type of policy", false, false);
            return new Result<bool>("", true, false);
        }

        public Result<bool> RemovePolicy(string id)
        {
            return new Result<bool>("", true, false);
        }

        public Result<IPurchasePolicyData> GetData()
        {
            return new Result<IPurchasePolicyData>("", true, new RestrictedHoursPolicyData(StartRestrict, EndRestrict, ProductId, Id));
        }

        public Result<bool> EditPolicy(Dictionary<string, object> info, string id)
        {
            if (Id != id)
                return new Result<bool>("", true, false);

            if (info.ContainsKey("StartRestrict"))
                StartRestrict  = (TimeSpan)info["StartRestrict"];

            if (info.ContainsKey("EndRestrict"))
                EndRestrict = (TimeSpan)info["EndRestrict"];

            if (info.ContainsKey("Product"))
                ProductId = (string)info["Product"];

            return new Result<bool>("", true, true);
        }
    }
}
