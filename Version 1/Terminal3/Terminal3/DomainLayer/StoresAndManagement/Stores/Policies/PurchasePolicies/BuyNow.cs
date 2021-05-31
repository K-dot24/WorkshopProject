using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DataAccessLayer.DTOs;


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

        public Result<IPurchasePolicy> RemovePolicy(string id)
        {
            if (Policy.Id.Equals(id))
            {
                IPurchasePolicy temp = this.Policy;
                this.Policy = new AndPolicy();
                return new Result<IPurchasePolicy>("", true, temp);
            }
            return Policy.RemovePolicy(id);            
        }

        public Result<IPurchasePolicyData> GetData()
        {
            return new Result<IPurchasePolicyData>("", true, new BuyNowData((AndPolicyData)Policy.GetData().Data, Id));
        }

        public Result<bool> EditPolicy(Dictionary<string, object> info, string id)
        {
            if(Id.Equals(id))
                return new Result<bool>("Can't edit the main purchase type", false, false);
            if (Policy.Id.Equals(id))            
                return new Result<bool>("Can't edit the main 'And node'", false, false);            
            return Policy.EditPolicy(info, id);
        }

        public DTO_BuyNow getDTO()
        {
            return new DTO_BuyNow(this.Id, new DTO_AndPolicy(this.Policy.Id, Policy.getPolicis_dto()));
        }
    }
}
