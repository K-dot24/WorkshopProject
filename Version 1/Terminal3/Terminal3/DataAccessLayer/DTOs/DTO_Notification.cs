using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_Notification
    {
        [BsonElement]
        public int EventName { get; set; }
        [BsonElement]
        public String Message { get; set; }
        [BsonElement]
        public String Date { get; set; }
        [BsonElement]
        public Boolean isOpened { get; set; }
        [BsonElement]
        public Boolean isStoreStaff { get; set; }
        [BsonElement]
        public String ClientId { get; set; }

        public DTO_Notification(int eventName, String message, String date, Boolean isOpened, Boolean isStoreStaff, String clientId)
        {
            EventName = eventName;
            Message = message;
            Date = date;
            this.isOpened = isOpened;
            this.isStoreStaff = isStoreStaff;
            ClientId = clientId;
        }
    }
}
