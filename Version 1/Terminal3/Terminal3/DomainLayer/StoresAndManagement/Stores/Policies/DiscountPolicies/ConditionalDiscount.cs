using System;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class ConditionalDiscount : VisibleDiscount
    {
        public int ConditionalQuantity { get; }

        public ConditionalDiscount(double precentege, DateTime expirationDate, int conditionalQuantity) : base(precentege, expirationDate)
        {
            ConditionalQuantity = conditionalQuantity;
        }

    }
}
