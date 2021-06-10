using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class Offer
    {
        //TODO: Complete properly

        public Tuple<Double, string> LastOffer { get; }    // Customer offer <price, UserID>
        public double CounterOffer { get; set; }    // Store offer
        public bool Accepted { get; }
        public string UserID { get; }
        public string ProductID { get; }
        public string StoreID { get; }
        public int Amount { get; }
        public double Price { get; }
        public string Id { get; set; }

        public Offer()
        {
            Id = Service.GenerateId();
            LastOffer = new Tuple<Double, String>(-1, null);
            Accepted = false;
        }
        public Offer(string userID, string productID, int amount, double price, string id = "", double counterOffer = -1.0, bool accepted = false)
        {           
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.CounterOffer = counterOffer;
            this.Accepted = accepted;
            this.UserID = userID;
            this.ProductID = productID;
            this.Amount = amount;
            this.Price = price;           
        }
        public Offer(string id , Tuple<double, string> lastOffer, double counterOffer, bool accepted )
        {
            Id = id;
            LastOffer = lastOffer;
            CounterOffer = counterOffer;
            Accepted = accepted;
        }

        public Result<bool> IsAccepted()
        {
            if (Accepted)
                return new Result<bool>("", true, true);
            if(CounterOffer == -1)
                return new Result<bool>("", false, false);
            return new Result<bool>("", true, false);
        }

        public Result<IDictionary<string, object>> GetData()
        {
            throw new NotImplementedException();
        }

        public DTO_Offer getDTO()
        {
            return new DTO_Offer(this.Id, this.LastOffer.Item1, this.LastOffer.Item2, this.CounterOffer, this.Accepted);
        }

        public static Result<Offer> Create(Dictionary<string, object> info)
        {
            string errorMsg = "Can't create Offer: ";
            string Id = "";
            double CounterOffer = -1.0;
            bool Accepted = false;
            if (!info.ContainsKey("UserID"))
                return new Result<Offer>(errorMsg + "UserID not found", false, null);
            string UserID = ((JsonElement)info["UserID"]).GetString();

            if (!info.ContainsKey("ProductId"))
                return new Result<Offer>(errorMsg + "ProductId not found", false, null);
            String productId = ((JsonElement)info["ProductId"]).GetString();

            if (!info.ContainsKey("Amount"))
                return new Result<Offer>(errorMsg + "Amount not found", false, null);
            int Amount = ((JsonElement)info["Amount"]).GetInt32();

            if (!info.ContainsKey("Price"))
                return new Result<Offer>(errorMsg + "Price not found", false, null);
            double Price = ((JsonElement)info["Price"]).GetDouble();

            if (info.ContainsKey("Id"))
                Id = ((JsonElement)info["Id"]).GetString();

            if (info.ContainsKey("CounterOffer"))                
                CounterOffer = ((JsonElement)info["CounterOffer"]).GetDouble();

            if (info.ContainsKey("Accepted"))
                Accepted = ((JsonElement)info["Accepted"]).GetBoolean();

            return new Result<Offer>("", true, new Offer(UserID, productId, Amount, Price, Id, CounterOffer, Accepted));
        }
    }
}
