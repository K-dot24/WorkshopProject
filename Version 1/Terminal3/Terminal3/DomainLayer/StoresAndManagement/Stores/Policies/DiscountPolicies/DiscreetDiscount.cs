using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscreetDiscount : AbstractDiscountPolicy
    {

        public String DiscountCode { set; get; }
        public IDiscountPolicy Discount { set; get; }

        public DiscreetDiscount(IDiscountPolicy discount, String discountCode, String id = "") : base(new Dictionary<string, object>(), id)
        {
            Discount = discount;
            DiscountCode = discountCode;
        }

        public static Result<IDiscountPolicy> create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create DiscreetDiscount: ";
            if (!info.ContainsKey("DiscountCode"))
                return new Result<IDiscountPolicy>(errorMsg + "DiscountCode not found", false, null);
            String discountCode = ((JsonElement)info["DiscountCode"]).GetString();

            return new Result<IDiscountPolicy>("", true, new DiscreetDiscount(null, discountCode));
        }

        public override Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            if (DiscountCode.Equals(code) && Discount != null)
                return Discount.CalculateDiscount(products, code);
            return new Result<Dictionary<Product, double>>("", false, new Dictionary<Product, double>());
        }

        public override Result<bool> AddDiscount(String id, IDiscountPolicy discount)
        {
            if (Id.Equals(id))
                //return new Result<bool>("Can't add a discount to a visible discount with an id " + id, false, false);
                Discount = discount;
            if(Discount != null)
                return Discount.AddDiscount(id, discount);
            return new Result<bool>("", true, false);
        }

        public override Result<IDiscountPolicy> RemoveDiscount(String id)
        {
            if (Discount != null)
                return Discount.RemoveDiscount(id);
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

        public override Result<IDictionary<string, object>> GetData()
        {
            IDictionary<string, object> dict = new Dictionary<string, object>() { 
                {"type", "DiscreeteDiscount" }, 
                {"Id", Id }, 
                {"DiscountCode", DiscountCode } , 
                {"Discount", null } 
            };
            if (Discount != null)
            {
                Result<IDictionary<string, object>> discountDataResult = Discount.GetData();
                if (!discountDataResult.ExecStatus)
                    return discountDataResult;
                dict["Discount"] = discountDataResult.Data;
                
            }           
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

            if (info.ContainsKey("DiscountCode"))
                DiscountCode = ((JsonElement)info["DiscountCode"]).GetString();

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
