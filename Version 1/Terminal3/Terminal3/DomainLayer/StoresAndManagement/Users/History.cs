using System.Collections.Generic;
using Terminal3.ServiceLayer.ServiceObjects;
using System.Collections.Concurrent;
using System;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DataAccessLayer.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;
using Terminal3.DataAccessLayer;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class History
    {
        public LinkedList<ShoppingBagService> ShoppingBags { get; }

        public History()
        {
            ShoppingBags = new LinkedList<ShoppingBagService>();
        }

        public History(LinkedList<ShoppingBagService> shoppingBags)
        {
            this.ShoppingBags = shoppingBags;
        }


        public void AddPurchasedShoppingCart(ShoppingCart shoppingCart)
        {
            ConcurrentDictionary<String, ShoppingBag> bags = shoppingCart.ShoppingBags;

            foreach (var bag in bags)
            {
                ShoppingBagService shoppingBagService = bag.Value.GetDAL().Data;
                ShoppingBags.AddLast(shoppingBagService);

                var filter = Builders<BsonDocument>.Filter.Eq("_id", bag.Value.User.Id);
                var update_history = Builders<BsonDocument>.Update.Push("History.ShoppingBags", GetDTO_HistoryShoppingBag(shoppingBagService));
                
                Mapper.getInstance().UpdateRegisteredUser(filter, update_history);
            }
        }

        public void AddPurchasedShoppingBag(ShoppingBag shoppingBag)
        {
            ShoppingBagService shoppingBagService = shoppingBag.GetDAL().Data;
            ShoppingBags.AddLast(shoppingBagService);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", shoppingBag.Store.Id);
            var update_history = Builders<BsonDocument>.Update.Push("History.ShoppingBags", GetDTO_HistoryShoppingBag(shoppingBagService));
            Mapper.getInstance().UpdateStore(filter, update_history);

        }

        public Result<HistoryService> GetDAL()
        {
            return new Result<HistoryService>("History DAL object", true, new HistoryService(ShoppingBags));
        }

        public DTO_History getDTO()
        {
            LinkedList<DTO_HistoryShoppingBag> hsp_dto = new LinkedList<DTO_HistoryShoppingBag>();
            foreach (var sb in ShoppingBags)
            {                
                hsp_dto.AddLast(GetDTO_HistoryShoppingBag(sb));
            }
            return new DTO_History(hsp_dto);
        }

        public DTO_HistoryShoppingBag GetDTO_HistoryShoppingBag(ShoppingBagService sb)
        {
            LinkedList<DTO_HistoryProduct> products_dto = new LinkedList<DTO_HistoryProduct>();
            foreach (var tup in sb.Products)
            {
                ProductService p = tup.Item1;
                DTO_HistoryProduct hp_dto = new DTO_HistoryProduct(p.Id, p.Name, p.Price, tup.Item2, p.Category);
                products_dto.AddLast(hp_dto);
            }
            return new DTO_HistoryShoppingBag(sb.Id, sb.UserId, sb.StoreId, products_dto, sb.TotalBagPrice);
        }
    }
}
