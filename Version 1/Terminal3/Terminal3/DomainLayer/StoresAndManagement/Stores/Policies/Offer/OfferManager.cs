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

        private Offer getOffer(string id)
        {
            foreach (Offer offer in PendingOffers)
                if (offer.Id.Equals(id))
                    return offer;
            return null;
        }

        public Result<Tuple<OfferResponse, Offer>> SendOfferResponseToUser(string userID, string offerID, bool accepted, double counterOffer, List<string> allOwners)
        {
            Offer offer = getOffer(offerID);
            if (offer == null)
                return new Result<Tuple<OfferResponse, Offer>>("Failed to response to an offer: Failed to locate the offer", false, null);
            if (accepted)
                return AcceptedResponse(userID, offer, allOwners);

            PendingOffers.Remove(offer);
            //TODO mapper?
            if (counterOffer == -1)
                return DeclinedResponse(offer);
            return CounterOfferResponse(offer, counterOffer);
        }

        private Result<Tuple<OfferResponse, Offer>> CounterOfferResponse(Offer offer, double counterOffer)
        {
            offer.CounterOffer = counterOffer;
            return new Result<Tuple<OfferResponse, Offer>>("", true, new Tuple<OfferResponse, Offer>(OfferResponse.CounterOffered, offer));
        }

        private Result<Tuple<OfferResponse, Offer>> DeclinedResponse(Offer offer)
        {
            return new Result<Tuple<OfferResponse, Offer>>("", true, new Tuple<OfferResponse, Offer>(OfferResponse.Declined, offer));
        }

        private Result<Tuple<OfferResponse, Offer>> AcceptedResponse(string userID, Offer offer, List<string> allOwners)
        {
            Result<Tuple<OfferResponse, Offer>> response = offer.AcceptedResponse(userID, allOwners);
            if (!response.ExecStatus)
                return response;
            if(response.Data.Item1 == OfferResponse.Accepted)
            {
                PendingOffers.Remove(offer);
                //TODo mapper?
            }
            return response;
        }

        internal void AddOffer(Offer offer)
        {
            PendingOffers.Add(offer);
        }
    }
}
