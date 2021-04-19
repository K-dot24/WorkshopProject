using System;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies
{
    public interface IPolicyManager
    {
    }

    public class PolicyManager : IPurchasePolicy
    {
        //TODO
        internal double GetCurrentProductPrice(Product product, int quantity)
        {
            //TODO
            //throw new NotImplementedException();
            return product.Price*quantity;
        }
    }
}
