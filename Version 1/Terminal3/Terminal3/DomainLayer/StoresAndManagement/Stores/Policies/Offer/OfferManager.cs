using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DataAccessLayer.DTOs;
using System.Threading;
using Terminal3.DataAccessLayer;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Offer
{

    public enum OfferResponse
    {
        None,
        Accepted,
        Declined,
        CounterOffered
    }

    public class OfferManager
    {

        public List<Offer> PendingOffers { get; set; }

        public OfferManager()
        {
            PendingOffers = new List<Offer>();
        }

        public OfferManager(List<Offer> pendingOffers)
        {
            PendingOffers = pendingOffers;
        }

        private Offer getOffer(string id)
        {
            foreach (Offer offer in PendingOffers)
                if (offer.Id.Equals(id))
                    return offer;
            return null;
        }

        public Result<OfferResponse> SendOfferResponseToUser(string ownerID, string offerID, bool accepted, double counterOffer, List<string> allOwners)
        {
            try
            {
                Monitor.Enter(offerID);
                try
                {
                    Offer offer = getOffer(offerID);
                    if (offer == null)
                        return new Result<OfferResponse>("Failed to response to an offer: Failed to locate the offer", false, OfferResponse.None);
                    if (accepted)
                        return AcceptedResponse(ownerID, offer, allOwners);

                    PendingOffers.Remove(offer);
                    
                    if (counterOffer == -1)
                    {
                        return DeclinedResponse(offer);
                    }

                    Result<OfferResponse> res = CounterOfferResponse(offer, counterOffer);
                    //Zoe - check counter updated in db
                    return res;
                }
                finally
                {
                    Monitor.Exit(offerID);
                }
            }catch(SynchronizationLockException SyncEx)
            {
                Console.WriteLine("A SynchronizationLockException occurred. Message:");
                Console.WriteLine(SyncEx.Message);
                return new Result<OfferResponse>(SyncEx.Message, false, OfferResponse.None);
            }
        }

        private Result<OfferResponse> CounterOfferResponse(Offer offer, double counterOffer)
        {
            offer.CounterOffer = counterOffer;
            return new Result<OfferResponse>("", true, OfferResponse.CounterOffered);
        }

        private Result<OfferResponse> DeclinedResponse(Offer offer)
        {
            return new Result<OfferResponse>("", true, OfferResponse.Declined);
        }

        private Result<OfferResponse> AcceptedResponse(string ownerID, Offer offer, List<string> allOwners)
        {
            Result<OfferResponse> response = offer.AcceptedResponse(ownerID, allOwners);
            if (!response.ExecStatus)
                return response;
            if(response.Data == OfferResponse.Accepted)
            {
                PendingOffers.Remove(offer);
            }
            return response;
        }

        internal void AddOffer(Offer offer)
        {
            PendingOffers.Add(offer);
        }

        public Result<List<Dictionary<string, object>>> getStoreOffers()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (Offer offer in PendingOffers)
                list.Add(offer.GetData());
            return new Result<List<Dictionary<string, object>>>("", true, list);
        }

        public List<DTO_Offer> GetDTO()
        {
            List<DTO_Offer> dto_offers = new List<DTO_Offer>();
            foreach(Offer offer in PendingOffers)
            {
                dto_offers.Add(new DTO_Offer(offer.Id, offer.UserID, offer.ProductID, offer.StoreID, offer.Amount, offer.Price, offer.CounterOffer, offer.acceptedOwners));
            }
            return dto_offers;
        }
    }
}
