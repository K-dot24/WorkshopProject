
namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public interface IPurchasePolicy
    {
        //TODO: Complete properly

        Result<double> CalculatePrice(Product product, int quantity);
    }
}
