using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData
{
    class DiscountTargetProductsData : IDiscountTargetData
    {

        public List<ProductService> Products { get; }

        public DiscountTargetProductsData(List<ProductService> products)
        {
            Products = products;
        }

    }
}
