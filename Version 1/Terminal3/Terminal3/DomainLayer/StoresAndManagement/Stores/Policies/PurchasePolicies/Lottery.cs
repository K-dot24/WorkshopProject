using System;
using System.Collections.Concurrent;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class Lottery : IPurchasePolicy
    {
        //TODO: Complete properly

        public Double Price { get; }
        public ConcurrentDictionary<String, Double> Participants { get; set; }  // <UserID, winning %>

        public Lottery(double price)
        {
            Price = price;
            Participants = new ConcurrentDictionary<string, double>();
        }

        public Result<double> CalculatePrice(Product product, int quantity)
        {
            throw new System.NotImplementedException();
        }
    }
}
