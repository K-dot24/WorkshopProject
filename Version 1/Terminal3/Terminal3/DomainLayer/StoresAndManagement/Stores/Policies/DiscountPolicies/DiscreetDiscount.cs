using System;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class DiscreetDiscount : VisibleDiscount
    {
        //TODO: Complete properly

        public String DiscountCode { get; }

        public DiscreetDiscount(double precentege, DateTime expirationDate, String discountCode) : base(precentege, expirationDate)
        {
            DiscountCode = discountCode;
        }

        public new Result<Double> CalculatePrice(Product product, User user, int quantity, String code)
        {
            //TODO: Add check if user is eligible?
            if (DiscountCode.Equals(code))
            {
                return base.CalculatePrice(product, user, quantity, code);
            }
            //else
            return new Result<Double>($"Discount for {product.Name} is over.\n", false, product.Price * quantity);     //return -1 ?
        }

    }
}
