using System;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class BuyNow : IPolicy
    {
        //TODO

        public Double BuyNowPrice { get; }

        public BuyNow(Double buyNowPrice)
        {
            BuyNowPrice = buyNowPrice;
        }

        public Result<double> CalculatePrice(Product product, int quantity)
        {
            return new Result<double>($"Calculated price for {product.Name}.\n", true, BuyNowPrice * quantity);
        }

        public Result<double> CalculatePrice(Product product, User user)
        {
            return null;
        }
    }
}
