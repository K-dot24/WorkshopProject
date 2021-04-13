using System;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class VisibleDiscount : IDiscountPolicy
    {
        public Double Precentege { get; }
        public DateTime ExpirationDate { get; }

        public VisibleDiscount(double precentege, DateTime expirationDate)
        {
            Precentege = precentege;
            ExpirationDate = expirationDate;
        }
    }
}
