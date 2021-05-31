using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class Offer : IPurchasePolicyType
    {
        //TODO: Complete properly

        public Tuple<Double, String> LastOffer { get; }    // Customer offer <price, UserID>
        public Double CounterOffer { get; set; }    // Store offer
        public Boolean Accepted { get; }

        public string Id { get; set; }

        public Offer()
        {
            Id = Service.GenerateId();
            LastOffer = new Tuple<Double, String>(-1, null);
            Accepted = false;
        }

        public Offer(string id , Tuple<double, string> lastOffer, double counterOffer, bool accepted )
        {
            Id = id;
            LastOffer = lastOffer;
            CounterOffer = counterOffer;
            Accepted = accepted;
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

        public Result<IPurchasePolicy> RemovePolicy(string id)
        {
            throw new NotImplementedException();
        }

        public Result<IPurchasePolicyData> GetData()
        {
            throw new NotImplementedException();
        }

        public Result<bool> EditPolicy(Dictionary<string, object> info, string id)
        {
            throw new NotImplementedException();
        }

        public DTO_Offer getDTO()
        {
            return new DTO_Offer(this.Id, this.LastOffer.Item1, this.LastOffer.Item2, this.CounterOffer, this.Accepted);
        }
    }
}
