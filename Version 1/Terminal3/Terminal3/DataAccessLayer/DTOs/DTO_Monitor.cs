using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;

namespace Terminal3.DataAccessLayer.DTOs
{
    class DTO_Monitor
    {
        [BsonId]
        public String Date { get; set; }
        [BsonElement]
        public int GuestUsers { get; set; }
        [BsonElement]
        public int RegisteredUsers { get; set; }
        [BsonElement]
        public int ManagersNotOwners { get; set; }
        [BsonElement]
        public int Owners { get; set; }
        [BsonElement]
        public int Admins { get; set; }

        public DTO_Monitor(string date, int guestUsers, int registeredUsers, int managersNotOwners, int owners, int admins)
        {
            Date = date;
            GuestUsers = guestUsers;
            RegisteredUsers = registeredUsers;
            ManagersNotOwners = managersNotOwners;
            Owners = owners;
            Admins = admins;
        }
    }
}
