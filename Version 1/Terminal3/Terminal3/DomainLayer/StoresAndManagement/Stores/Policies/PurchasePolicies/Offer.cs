using System;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class Offer : IPurchasePolicy
    {
        //TODO: Complete properly

        public Tuple<Double, String> LastOffer { get; }    // Customer offer <price, UserID>
        public Double CounterOffer { get; set; }    // Store offer
        public Boolean Accepted { get; }

        public string Id => throw new NotImplementedException();

        public Offer()
        {
            LastOffer = new Tuple<Double, String>(-1, null);
            Accepted = false;
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

        public Result<IPurchasePolicyData> GetData()
        {
            throw new NotImplementedException();
        }
    }
}
