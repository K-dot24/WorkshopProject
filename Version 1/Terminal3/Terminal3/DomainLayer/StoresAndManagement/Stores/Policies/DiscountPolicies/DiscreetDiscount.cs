using System;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscreetDiscount : VisibleDiscount
    {
        public String DiscountCode { get; }

        public DiscreetDiscount(double precentege, DateTime expirationDate, String discountCode) : base(precentege, expirationDate)
        {
            DiscountCode = discountCode;
        }
        
    }
}
