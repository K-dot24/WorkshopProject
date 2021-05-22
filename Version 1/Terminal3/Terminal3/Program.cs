using MongoDB.Bson;
using MongoDB.Driver;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Terminal3.DataAccessLayer;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.ServiceLayer;

namespace Terminal3
{
    class Program
    {        
        static void Main(string[] args)
        {
            //RegisteredUser zoe = new RegisteredUser("zoe.ellins@gmail.com", "password");
            //Mapper.getInstance().Create(zoe);
            //RegisteredUser shmar = new RegisteredUser("shmar@gmail.com", "shmar");
            //Mapper.getInstance().Create(shmar);

            //Store store = new Store("TestStore", zoe);
            //Mapper.getInstance().Create(store);
            //Mapper.getInstance().Create(store.Founder);

            //StoreManager manager = new StoreManager(shmar, store, new Permission(), store.Founder);
            //Mapper.getInstance().Create(manager);
            //store.Founder.StoreManagers.AddLast(manager);
            //var filter2 = Builders<BsonDocument>.Filter.Eq("UserId", store.Founder.GetId()) & Builders<BsonDocument>.Filter.Eq("StoreId", store.Founder.Store.Id); ;
            //var update = Builders<BsonDocument>.Update.Set("StoreManagers", Mapper.getInstance().Get_DTO_ManagerList(store.Founder.StoreManagers));
            //Mapper.getInstance().UpdateStoreOwner(filter2, update);

            //var m_filter = Builders<BsonDocument>.Filter.Eq("UserId", manager.GetId())& Builders<BsonDocument>.Filter.Eq("StoreId", manager.Store.Id);
            //var o_filter = Builders<BsonDocument>.Filter.Eq("UserId", store.Founder.GetId()) & Builders<BsonDocument>.Filter.Eq("StoreId", store.Founder.Store.Id);
            //var z_filter = Builders<BsonDocument>.Filter.Eq("_id", zoe.Id);
            //var s_filter = Builders<BsonDocument>.Filter.Eq("_id", shmar.Id);

            var m_filter = Builders<BsonDocument>.Filter.Eq("UserId", "981567a26c224326a696d8f92aacc914") & Builders<BsonDocument>.Filter.Eq("StoreId", "c8fd8b4fade94c7c8aa8ba02e2b4a290");
            var o_filter = Builders<BsonDocument>.Filter.Eq("UserId", "4d43053d5b7f4f5c936dd24df5b0bac8") & Builders<BsonDocument>.Filter.Eq("StoreId", "c8fd8b4fade94c7c8aa8ba02e2b4a290");
            var z_filter = Builders<BsonDocument>.Filter.Eq("_id", "4d43053d5b7f4f5c936dd24df5b0bac8");
            var s_filter = Builders<BsonDocument>.Filter.Eq("_id", "981567a26c224326a696d8f92aacc914");


            StoreManager load_manager = Mapper.getInstance().LoadStoreManager(m_filter);
            StoreOwner load_owner = Mapper.getInstance().LoadStoreOwner(o_filter);
            RegisteredUser load_zoe = Mapper.getInstance().LoadRegisteredUser(z_filter);
            RegisteredUser load_shmar = Mapper.getInstance().LoadRegisteredUser(s_filter);

            //Product product = new Product("Banana", 19.9, 10, "Fruit");
            //Mapper.getInstance().Create(product);

            //store.InventoryManager.Products.TryAdd(product.Id, product);
            



            //var filter2 = Builders<BsonDocument>.Filter.Eq("_id", ru.Id);
            //var update = Builders<BsonDocument>.Update.Set("ShoppingCart", Mapper.getInstance().Get_DTO_ShoppingCart(ru));
            //Mapper.getInstance().UpdateRegisteredUser(filter2, update);


            //IDictionary<String, Object> paymentDetails = new Dictionary<String, Object>();
            //IDictionary<String, Object> deliveryDetails = new Dictionary<String, Object>();

            //ru.Purchase(paymentDetails, deliveryDetails);

            //filter2 = Builders<BsonDocument>.Filter.Eq("_id", ru.Id);
            //update = Builders<BsonDocument>.Update.Set("History", Mapper.getInstance().Get_DTO_History(ru.History));
            //Mapper.getInstance().UpdateRegisteredUser(filter2, update);

            //RegisteredUser fromdb = Mapper.getInstance().LoadRegisteredUser(filter2);

            Console.WriteLine("done");

            //GuestUser gu = new GuestUser();
            //String gu_id = gu.Id;

            //Mapper.getInstance().Create(gu);
            //var filter = Builders<BsonDocument>.Filter.Eq("_id", gu.Id);
            //GuestUser guest = Mapper.getInstance().LoadGuestUser(filter);



            //var update = Builders<BsonDocument>.Update.Set("Password", "zoe");
            //ruser.Password = "zoe";
            //Mapper.getInstance().UpdateRegisteredUser(filter2 , update) ;

            //Mapper.getInstance().DeleteRegisteredUser(filter2);

            //Thread.Sleep(1000);
            //ECommerceSystem a = new ECommerceSystem();
            ////HubConnection SignalRClient = new HubConnection("http://localhost:8080/signalr");
            ////SignalRClient.CreateHubProxy("NotificationHub");
            ////SignalRClient.Start();
            //Console.WriteLine("Hello World!");
            //Console.ReadKey();

        }
    }
}
