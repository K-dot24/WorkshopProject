using System;
using System.Collections.Generic;
using System.Text;

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

        List<Offer> PendingOffers { get; }

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
            Offer offer = getOffer(offerID);
            if (offer == null)
                return new Result<OfferResponse>("Failed to response to an offer: Failed to locate the offer", false, OfferResponse.None);
            if (accepted)
                return AcceptedResponse(ownerID, offer, allOwners);

            PendingOffers.Remove(offer);
            //TODO mapper Zoe
            if (counterOffer == -1)
                return DeclinedResponse(offer);
            return CounterOfferResponse(offer, counterOffer);
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
                //TODO mapper Zoe
            }
            return response;
        }

        internal void AddOffer(Offer offer)
        {
            PendingOffers.Add(offer);
            //TODO: mapper Zoe
        }

        public Result<List<Dictionary<string, object>>> getStoreOffers()
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (Offer offer in PendingOffers)
                list.Add(offer.GetData());
            return new Result<List<Dictionary<string, object>>>("", true, list);
        }
    }
}
