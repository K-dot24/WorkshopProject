using System.Collections.Generic;
using Terminal3.ServiceLayer.ServiceObjects;
using System.Collections.Concurrent;
using System;
using Terminal3.DomainLayer.StoresAndManagement.Users;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.DataAccessLayer.DTOs;

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
                ShoppingBags.AddLast(bag.Value.GetDAL().Data);
            }
        }

        public void AddPurchasedShoppingBag(ShoppingBag shoppingBag)
        {
            ShoppingBags.AddLast(shoppingBag.GetDAL().Data);
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
                LinkedList<DTO_HistoryProduct> products_dto = new LinkedList<DTO_HistoryProduct>();
                foreach(var tup in sb.Products)
                {
                    ProductService p = tup.Item1;
                    DTO_HistoryProduct hp_dto = new DTO_HistoryProduct(p.Id, p.Name, p.Price, tup.Item2, p.Category); // TODO- item2 or product service.quatity ?
                    products_dto.AddLast(hp_dto);
                }
                hsp_dto.AddLast(new DTO_HistoryShoppingBag(sb.Id, sb.UserId, sb.StoreId, products_dto, sb.TotalBagPrice));
            }
            return new DTO_History(hsp_dto);
        }
    }
}
