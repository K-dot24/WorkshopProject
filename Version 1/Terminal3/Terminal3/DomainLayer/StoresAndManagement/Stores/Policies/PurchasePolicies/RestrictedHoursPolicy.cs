using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies
{
    class RestrictedHoursPolicy : IPurchasePolicy
    {
        public TimeSpan StartRestrict { get; }
        public TimeSpan EndRestrict { get; }
        public Product Product { get; }
        public RestrictedHoursPolicy(TimeSpan startRestrict, TimeSpan endRestrict, Product product)
        {
            this.StartRestrict = startRestrict;
            this.EndRestrict= endRestrict;
            this.Product = product;
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            return new Result<bool>("", true, now > EndRestrict && now < StartRestrict);
        }
    }
}
