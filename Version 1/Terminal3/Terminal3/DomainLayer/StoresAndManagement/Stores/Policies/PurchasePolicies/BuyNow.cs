using System;
using System.Collections.Concurrent;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    public class BuyNow : IPurchasePolicyType
    {

        public AndPolicy Policy { get; set; }
        public string Id { get; }

        public BuyNow(string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.Policy = new AndPolicy();
        }

        public BuyNow(AndPolicy policy, string id)
        {
            Policy = policy;
            Id = id;
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {            
            if(this.Policy.IsConditionMet(bag, user).Data)
                return new Result<bool>("",true,true);
            return new Result<bool>("Policy conditions not met", true, false); 
        }

        public Result<bool> AddPolicy(IPurchasePolicy policy, string id)
        {
            return this.Policy.AddPolicy(policy, id);
        }

        public Result<bool> RemovePolicy(string id)
        {
            if (Policy.Id.Equals(id))
            {
                this.Policy = new AndPolicy();
                return new Result<bool>("", true, true);
            }
            return Policy.RemovePolicy(id);            
        }

        public Result<IPurchasePolicyData> GetData()
        {
            return new Result<IPurchasePolicyData>("", true, new BuyNowData((AndPolicyData)Policy.GetData().Data, Id));
        }

        public Result<bool> EditPolicy(IPurchasePolicy policy, string id)
        {
            if(Id.Equals(id))
                return new Result<bool>("Can't edit the main purchase type", false, false);
            if (Policy.Id.Equals(id))            
                return new Result<bool>("Can't edit the main 'And node'", false, false);            
            return Policy.EditPolicy(policy, id);
        }
    }
}
