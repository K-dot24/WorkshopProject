using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Offer
{
    public class Offer
    {
        public string Id { get; set; }
        public string UserID { get; }
        public string ProductID { get; }
        public string StoreID { get; }
        public int Amount { get; }
        public double Price { get; }

        public double CounterOffer { get; set; }

        public List<string> acceptedOwners { get; }

        public Offer(string userID, string productID, int amount, double price, string storeID, string id = "")
        {           
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.UserID = userID;
            this.ProductID = productID;
            this.StoreID = storeID;
            this.Amount = amount;
            this.Price = price;
            this.CounterOffer = -1;
            this.acceptedOwners = new List<string>();
        }

        public DTO_Offer getDTO()
        {
            return new DTO_Offer(this.Id, this.LastOffer.Item1, this.LastOffer.Item2, this.CounterOffer, this.Accepted);
        }



        private bool didAllOwnersAccept(List<string> allOwners)
        {
            foreach (string id in allOwners)
                if (!acceptedOwners.Contains(id))
                    return false;
            return true;
        }

        public Result<OfferResponse> AcceptedResponse(string ownerID, List<string> allOwners)
        {
            if (acceptedOwners.Contains(ownerID))
                return new Result<OfferResponse>("Failed to response to an offer: Owner Can't accept an offer more than once", false, OfferResponse.None);
            acceptedOwners.Add(ownerID);
            if (didAllOwnersAccept(allOwners))
                return new Result<OfferResponse>("", true, OfferResponse.Accepted);
            else
                return new Result<OfferResponse>("", true, OfferResponse.None);
        }

        public Dictionary<string, object> GetData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>() {
                { "Id", Id },
                { "Product", ProductID },
                { "User", UserID },
                { "Store", StoreID },
                { "Amount", Amount },
                { "Price", Price },
            };
            if (CounterOffer != -1)
                data.Add("Counter offer price", CounterOffer);
            return data;
        }
    }
}
