using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountData.DiscountTargetsData;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountTargets
{
    public class DiscountTargetProducts : IDiscountTarget
    {
        public String Id { get; set; }
        public List<Product> Products { get; }

        public DiscountTargetProducts(List<Product> products)
        {
            Id = Service.GenerateId();
            Products = products;
        }

        // for loading from db
        public DiscountTargetProducts(string id, List<Product> products)
        {
            Id = id;
            Products = products;
        }

        public List<Product> getTargets(ConcurrentDictionary<Product, int> products)
        {
            List<Product> result = new List<Product>();
            foreach(Product product in Products)
            {
                if (products.ContainsKey(product))
                    result.Add(product);
            }
            return result;
        }

        public Result<IDiscountTargetData> GetData()
        {
            List<ProductService> productList = new List<ProductService>();
            foreach(Product myProduct in Products)
            {
                Result<ProductService> productResult = myProduct.GetDAL();
                if (!productResult.ExecStatus)
                    return new Result<IDiscountTargetData>(productResult.Message, false, null);
                productList.Add(productResult.Data);
            }
            return new Result<IDiscountTargetData>("", true, new DiscountTargetProductsData(productList));
        }

        public string getId()
        {
            return this.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is DiscountTargetProducts products &&
                   Id == products.Id &&
                   EqualityComparer<List<Product>>.Default.Equals(Products, products.Products);
        }
    }
}
