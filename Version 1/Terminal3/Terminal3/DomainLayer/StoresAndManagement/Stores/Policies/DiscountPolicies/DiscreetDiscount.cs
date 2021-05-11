using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscreetDiscount : AbstractDiscountPolicy
    {
        //TODO: Complete properly

        public String DiscountCode { get; }
        public IDiscountPolicy Discount { get; }

        public DiscreetDiscount(IDiscountPolicy discount, String discountCode, String id = "") : base(id)
        {
            Discount = discount;
            DiscountCode = discountCode;
        }

        public override Result<Dictionary<Product, double>> CalculateDiscount(ConcurrentDictionary<Product, int> products, string code = "")
        {
            if (DiscountCode.Equals(code))
                return Discount.CalculateDiscount(products, code);
            return new Result<Dictionary<Product, double>>("", false, new Dictionary<Product, double>());
        }

        public override Result<bool> AddDiscount(String id, IDiscountPolicy discount)
        {
            if (Id.Equals(id))
                return new Result<bool>("Can't add a discount to a visible discount with an id " + id, false, false);
            return Discount.AddDiscount(id, discount);
        }

        public override Result<bool> RemoveDiscount(String id)
        {
            return Discount.RemoveDiscount(id);
        }

        public override Result<bool> AddCondition(string id, IDiscountCondition condition)
        {
            return new Result<bool>("", true, false);
        }

        public override Result<bool> RemoveCondition(string id)
        {
            return new Result<bool>("", true, false);
        }
    }
}
