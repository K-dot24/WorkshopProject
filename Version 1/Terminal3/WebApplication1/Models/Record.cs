using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRgateway.Models
{
    public class Record
    {
        string Date { get; set; }
        int GuestUsers { get; set; }
        int RegisteredUsers { get; set; }
        int ManagersNotOwners { get; set; }
        int Owners { get; set; }
        int Admins { get; set; }

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
