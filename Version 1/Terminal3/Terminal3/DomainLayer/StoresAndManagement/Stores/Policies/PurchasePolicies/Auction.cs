using System;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class Auction : IPurchasePolicy
    {
        //TODO: Complete properly

        public DateTime ClosingTime { get; }
        public Double StartingPrice { get; }
        public Tuple<Double, String> LastOffer { get; }    // Customer offer <price, UserID>

        public string Id => throw new NotImplementedException();

        public Auction(DateTime closingTime, Double startingPrice)
        {
            ClosingTime = closingTime;
            StartingPrice = startingPrice;
            LastOffer = new Tuple<Double, String>(-1, null);
        }

        public Result<double> CalculatePrice(Product product, int quantity)
        {
            throw new System.NotImplementedException();
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            throw new NotImplementedException();
        }

        public Result<bool> AddPolicy(IPurchasePolicy policy, string id)
        {
            throw new NotImplementedException();
        }

        public Result<bool> RemovePolicy(string id)
        {
            throw new NotImplementedException();
        }
    }
}
