using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DataAccessLayer.DTOs
{
    public class DTO_SystemAdmins
    {
        [BsonId]
        public String _id { get; set; }
        [BsonElement]
        public LinkedList<String> SystemAdmins { get; set; }

        public DTO_SystemAdmins(LinkedList<string> admins)
        {
            _id = ""; 
            SystemAdmins = admins;
        }
    }
}
