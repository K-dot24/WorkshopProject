using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class RestrictedHoursPolicy : IPurchasePolicy
    {
        public DateTime StartRestrict { get; set; }
        public DateTime EndRestrict { get; set; }
        public string ProductId { get; set; }
        public string Id { get; }

        public RestrictedHoursPolicy(DateTime startRestrict, DateTime endRestrict, string productId, string id = "")
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
            DateTime startRestrict= createDateTime((JsonElement)info["StartRestrict"]);

            if (!info.ContainsKey("EndRestrict"))
                return new Result<IPurchasePolicy>(errorMsg + "EndRestrict not found", false, null);
            DateTime endRestrict = createDateTime((JsonElement)info["EndRestrict"]);

            if (!info.ContainsKey("ProductId"))
                return new Result<IPurchasePolicy>(errorMsg + "ProductId not found", false, null);
            string productId = ((JsonElement)info["ProductId"]).GetString();

            return new Result<IPurchasePolicy>("", true, new RestrictedHoursPolicy(startRestrict, endRestrict, productId));
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            DateTime now = DateTime.Now;
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
                StartRestrict  = createDateTime((JsonElement)info["StartRestrict"]);

            if (info.ContainsKey("EndRestrict"))
                EndRestrict = createDateTime((JsonElement)info["EndRestrict"]);

            if (info.ContainsKey("ProductId"))
                ProductId = ((JsonElement)info["ProductId"]).GetString();

            return new Result<bool>("", true, true);
        }

        private static DateTime createDateTime(JsonElement timeElement)
        {
            String timeString = timeElement.GetString();
            DateTime time = DateTime.ParseExact(timeString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            return time;
        }

        public DTO_RestrictedHoursPolicy getDTO()
        {
            return new DTO_RestrictedHoursPolicy(this.Id, this.StartRestrict.ToString(), this.EndRestrict.ToString(), this.ProductId);
        }
    }
}
