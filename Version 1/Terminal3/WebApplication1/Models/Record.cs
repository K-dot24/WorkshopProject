using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRgateway.Models
{
    public class Record
    {
        public string Date { get; set; }
        public int GuestUsers { get; set; }
        public int RegisteredUsers { get; set; }
        public int ManagersNotOwners { get; set; }
        public int Owners { get; set; }
        public int Admins { get; set; }

        public Record(string date, int guestUsers, int registeredUsers, int managersNotOwners, int owners, int admins)
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
