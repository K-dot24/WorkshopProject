using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DataAccessLayer.DTOs;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class Lottery : IPurchasePolicyType
    {
        //TODO: Complete properly

        public Double Price { get; }
        public ConcurrentDictionary<String, Double> Participants { get; set; }  // <UserID, winning %>

        public string Id { get; set; }

        public Lottery(double price)
        {
            Id = Service.GenerateId();
            Price = price;
            Participants = new ConcurrentDictionary<string, double>();
        }

        public Lottery(string id ,double price, ConcurrentDictionary<string, double> participants)
        {
            Id = id;
            Price = price;
            Participants = participants;
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

        public Result<IDictionary<string, object>> GetData()
        {
            throw new NotImplementedException();
        }

        public Result<bool> EditPolicy(Dictionary<string, object> info, string id)
        {
            throw new NotImplementedException();
        }

        public DTO_Lottery getDTO()
        {
            return new DTO_Lottery(this.Id, this.Price, this.Participants);
        }
    }
}
