using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies
{
    public interface IDiscountPolicy
    {
        Result<double> CalculatePrice(Product product, User user);
    }
}
