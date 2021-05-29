using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_History
    {
        [BsonElement]
        public LinkedList<DTO_HistoryShoppingBag> ShoppingBags { get; set; }

        public DTO_History(LinkedList<DTO_HistoryShoppingBag> shoppingBags)
        {
            ShoppingBags = shoppingBags;
        }
    }
}
