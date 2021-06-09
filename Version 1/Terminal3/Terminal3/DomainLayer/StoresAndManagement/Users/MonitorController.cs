using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class MonitorController
    {
        private static MonitorController Instance = null;

        public List<Tuple<DateTime, String>> GuestUsers { get; set;}
        public List<Tuple<DateTime, String>> RegisteredUsers { get; set; }
        public List<Tuple<DateTime, String>> ManagersNotOwners { get; set; }
        public List<Tuple<DateTime, String>> Owners { get; set;}
        public List<Tuple<DateTime, String>> Admins { get; set; }

        private MonitorController()
        {
            GuestUsers = new List<Tuple<DateTime, string>>();
            RegisteredUsers = new List<Tuple<DateTime, string>>();
            ManagersNotOwners = new List<Tuple<DateTime, string>>();
            Owners = new List<Tuple<DateTime, string>>();
            Admins = new List<Tuple<DateTime, string>>();
        }

        public static MonitorController getInstance()
        {
            if (Instance == null)
            {
                Instance = new MonitorController();
            }
            return Instance;
        }

    }
}
