using System;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class Lottery : IPurchasePolicy
    {
        //TODO: Complete properly

        public Double Price { get; }
        public ConcurrentDictionary<String, Double> Participants { get; set; }  // <UserID, winning %>

        public string Id => throw new NotImplementedException();

        public Lottery(double price)
        {
            Price = price;
            Participants = new ConcurrentDictionary<string, double>();
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
