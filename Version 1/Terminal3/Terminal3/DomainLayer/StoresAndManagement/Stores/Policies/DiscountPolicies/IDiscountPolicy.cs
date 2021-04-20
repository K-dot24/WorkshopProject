using System;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public interface IDiscountPolicy
    {
        //TODO: Complete properly

        Result<Double> CalculatePrice(Product product, User user, int quantity, String code);
        Result<Boolean> CheckIfEligible(User user);
    }
}
