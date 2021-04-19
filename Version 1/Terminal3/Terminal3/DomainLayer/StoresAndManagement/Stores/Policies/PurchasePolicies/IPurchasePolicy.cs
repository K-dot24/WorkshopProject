using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public interface IPurchasePolicy
    {
        //TODO
        Result<double> CalculatePrice(Product product, int quantity);
    }

    /*public enum policyType : int
    {
        Auction = 0 ,
        BuyNow = 1,
        Lottery = 2 ,
        Offer = 3
    }*/
}
