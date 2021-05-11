using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData
{
    class DiscountTargetCategoriesData : IDiscountTargetData
    {
        public List<string> Categories { get; }

        public DiscountTargetCategoriesData(List<string> categories)
        {
            Categories = categories;
        }

    }
}
