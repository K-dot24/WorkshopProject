using System;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public class VisibleDiscount : IDiscountPolicy
    {
        //TODO: Complete properly

        public Double Precentege { get; }   // 0-100
        public DateTime ExpirationDate { get; }

        //TODO: Add who eligible parameters for the discount (maybe User/RegisteredUser?)

        public VisibleDiscount(double precentege, DateTime expirationDate)
        {
            Precentege = precentege;
            ExpirationDate = expirationDate;
        }

        public Result<Double> CalculatePrice(Product product, User user, int quantity, String code)
        {
            //TODO: Add check if user is eligible?
            if (DateTime.Now.CompareTo(ExpirationDate) < 0)
            {
                Double newPrice = product.Price * ((100 - Precentege) / 100);
                return new Result<Double>($"Calculated price for {product.Name}.\n", true, newPrice*quantity);
            }
            //else
            return new Result<Double>($"Discount for {product.Name} is over.\n", false, product.Price*quantity);     //return -1 ?
        }

        public Result<bool> CheckIfEligible(User user)
        {
            throw new NotImplementedException();
        }
    }
}
