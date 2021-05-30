using MongoDB.Bson;
using MongoDB.Driver;
using MongoRepository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Terminal3.DataAccessLayer;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.DiscountPolicies.DiscountTargets;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.PurchasePolicies;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer;

namespace Terminal3
{
    class Program
    {        
        static void Main(string[] args)
        {
            Mapper mapper = Mapper.getInstance();

            List<String> categories = new List<string>();
            categories.Add("category1");
            categories.Add("category2");
            categories.Add("category3");

            DiscountTargetCategories discount = new DiscountTargetCategories(categories);

            List<Product> products = new List<Product>();
            Product p1 = new Product("3ea1d92baf2c431084d1f70d5596c047", "p1", 10, 10, "test1", new LinkedList<string>(), new ConcurrentDictionary<string, string>());
            Product p2 = new Product("bad385d241ca4f0cbcd977b6642a99ce", "p2", 20, 10, "test2", new LinkedList<string>(), new ConcurrentDictionary<string, string>());
            Product p3 = new Product("3ea1d92baf2c431084d1f70d5596c047", "p3", 30, 10, "test3", new LinkedList<string>(), new ConcurrentDictionary<string, string>());
            Product p4 = new Product("3ea1d92baf2c431084d1f70d5596c047", "p4", 40, 10, "test4", new LinkedList<string>(), new ConcurrentDictionary<string, string>());

            products.Add(p1);
            products.Add(p2);
            DiscountTargetProducts discountTarget = new DiscountTargetProducts(products);
            mapper.Create(discountTarget);

            VisibleDiscount visibleDiscount = new VisibleDiscount(DateTime.Now, discount, 17);
            mapper.Create(visibleDiscount);

            var v_filter = Builders<BsonDocument>.Filter.Eq("_id", visibleDiscount.Id);
            VisibleDiscount load = mapper.LoadVisibleDiscount(v_filter);

            visibleDiscount.Target = discountTarget;
            ConcurrentDictionary<String, String> toReplace = new ConcurrentDictionary<string, string>();
            toReplace.TryAdd("DiscountTargetProducts", discountTarget.getId());
            var v_update = Builders<BsonDocument>.Update.Set("Target", toReplace);
            mapper.UpdateVisibleDiscount(v_filter, v_update);

            VisibleDiscount load2 = mapper.LoadVisibleDiscount(v_filter);

            Console.WriteLine();
            //mapper.DeleteVisibleDiscount(v_filter);









            /*
            Thread.Sleep(1000);
            ECommerceSystem a = new ECommerceSystem();
            //HubConnection SignalRClient = new HubConnection("http://localhost:8080/signalr");
            //SignalRClient.CreateHubProxy("NotificationHub");
            //SignalRClient.Start();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
            */
        }
    }
}
