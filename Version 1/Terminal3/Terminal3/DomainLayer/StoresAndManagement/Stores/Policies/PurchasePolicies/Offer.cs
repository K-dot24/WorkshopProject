using System;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class Offer : IPurchasePolicy
    {
        //TODO: Complete properly

        public Tuple<Double, String> LastOffer { get; }    // Customer offer <price, UserID>
        public Double CounterOffer { get; set; }    // Store offer
        public Boolean Accepted { get; }

        public Offer()
        {
            LastOffer = new Tuple<Double, String>(-1, null);
            Accepted = false;
        }

        public Result<double> CalculatePrice(Product product, int quantity)
        {
            throw new System.NotImplementedException();
        }
    }
}
