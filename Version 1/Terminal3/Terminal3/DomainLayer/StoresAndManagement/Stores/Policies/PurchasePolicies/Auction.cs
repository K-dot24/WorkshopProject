using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DataAccessLayer.DTOs;


namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class Auction : IPurchasePolicyType
    {
        //TODO: Complete properly

        public DateTime ClosingTime { get; }
        public Double StartingPrice { get; }
        public Tuple<Double, String> LastOffer { get; }    // Customer offer <price, UserID>

        public string Id { get; set; }

        public Auction(DateTime closingTime, Double startingPrice)
        {
            Id = Service.GenerateId();
            ClosingTime = closingTime;
            StartingPrice = startingPrice;
            LastOffer = new Tuple<Double, String>(-1, null);
        }

        public Auction(string id , String closingTime, double startingPrice, Tuple<double, string> lastOffer)
        {
            Id = id;
            ClosingTime = DateTime.Parse(closingTime);
            StartingPrice = startingPrice;
            LastOffer = lastOffer;
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

        public Result<bool> EditPolicy(Dictionary<string, object> info, string id)
        {
            throw new NotImplementedException();
        }

        public DTO_Auction getDTO()
        {
            return new DTO_Auction(this.Id, this.ClosingTime.ToString(), this.StartingPrice, this.LastOffer.Item1, this.LastOffer.Item2);
        }
    }
}
