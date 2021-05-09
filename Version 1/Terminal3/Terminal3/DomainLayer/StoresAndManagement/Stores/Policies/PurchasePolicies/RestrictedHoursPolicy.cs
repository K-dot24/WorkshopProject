﻿using System;
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
        public string Id { get; }

        public RestrictedHoursPolicy(TimeSpan startRestrict, TimeSpan endRestrict, Product product, string id = "")
        {
            this.Id = id;
            if (id.Equals(""))
                this.Id = Service.GenerateId();
            this.StartRestrict = startRestrict;
            this.EndRestrict= endRestrict;
            this.Product = product;
        }

        public Result<bool> IsConditionMet(ConcurrentDictionary<Product, int> bag, User user)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            return new Result<bool>("", true, now > EndRestrict && now < StartRestrict);
        }

        public Result<bool> AddPolicy(IPurchasePolicy policy, string id)
        {
            if (this.Id.Equals(id))
                return new Result<bool>("Cannot add a policy to this type of policy", false, false);
            return new Result<bool>("", true, false);
        }

        public Result<bool> RemovePolicy(string id)
        {
            return new Result<bool>("", true, false);
        }
    }
}
