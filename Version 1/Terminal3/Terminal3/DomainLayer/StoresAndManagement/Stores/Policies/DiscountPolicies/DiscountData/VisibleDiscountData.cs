using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData
{
    class VisibleDiscountData : AbstractDiscountPolicyData
    {

        public DateTime ExpirationDate { get; }
        public IDiscountTargetData Target { get; }
        public Double Percentage { get; }

        public VisibleDiscountData(DateTime expirationDate, IDiscountTargetData target, Double percentage, String id = "") : base(id)
        {
            ExpirationDate = expirationDate;
            Target = target;
            if (percentage > 100)
                Percentage = 100;
            else if (percentage < 0)
                Percentage = 0;
            else
                Percentage = percentage;
        }

    }
}
