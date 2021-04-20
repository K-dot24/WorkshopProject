using System;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class ConditionalDiscount : VisibleDiscount
    {
        //TODO: Complete properly

        public int ConditionalQuantity { get; }

        public ConditionalDiscount(double precentege, DateTime expirationDate, int conditionalQuantity) : base(precentege, expirationDate)
        {
            ConditionalQuantity = conditionalQuantity;
        }

        public new Result<Double> CalculatePrice(Product product, User user, int quantity, String code)
        {
            //TODO: Add check if user is eligible?
            if (quantity >= ConditionalQuantity)
            {
                return base.CalculatePrice(product, user, quantity, code);
            }
            //else
            return new Result<Double>($"Discount for {product.Name} is over.\n", false, product.Price * quantity);     //return -1 ?
        }

    }
}
