using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Terminal3.DataAccessLayer;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.ServiceLayer;

namespace Terminal3.ServiceLayer.Controllers
{
    public class MonitorController
    {
        private static MonitorController Instance = null;

        DateTime today;
        public int GuestUsers { get; set; }
        public int RegisteredUsers { get; set; }
        public int ManagersNotOwners { get; set; }
        public int Owners { get; set; }
        public int Admins { get; set; }

        public List<String> visitorsIDs { get; set; }

        private MonitorController()
        {
            DTO_Monitor dto = Mapper.getInstance().LoadMonitor();
            today = DateTime.Now.Date;
            GuestUsers = dto.GuestUsers;
            RegisteredUsers = dto.RegisteredUsers;
            ManagersNotOwners = dto.ManagersNotOwners;
            Owners = dto.Owners;
            Admins = dto.Admins;
            visitorsIDs = new List<string>();
        }

        public static MonitorController getInstance()
        {
            if (Instance == null)
            {
                Instance = new MonitorController();
            }
            return Instance;
        }

        public void updateForOpenStore(String fieldName, String userID)
        {
            if (DateTime.Now.Date > today)
            {
                GuestUsers = 0;
                RegisteredUsers = 0;
                ManagersNotOwners = 0;
                Owners = 0;
                Admins = 0;
                visitorsIDs.Clear();
            }
            switch (fieldName)
            {
                case "RegisteredUsers":
                    Owners++;
                    if (RegisteredUsers > 0)
                        RegisteredUsers--;
                    break;

                case "ManagersNotOwners":
                    Owners++;
                    if (ManagersNotOwners > 0)
                        ManagersNotOwners--;
                    break;
            }

            //save in DB
            sendStatus();
            String date = today.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            Mapper.getInstance().Update(new DTO_Monitor(date, GuestUsers, RegisteredUsers, ManagersNotOwners, Owners, Admins));
        }

        public void update(String fieldName,String userID)
        {
            if (DateTime.Now.Date > today)
            {
                GuestUsers = 0;
                RegisteredUsers = 0;
                ManagersNotOwners = 0;
                Owners = 0;
                Admins = 0;
                visitorsIDs.Clear();
            }
            //do not count the same user twice
            if (visitorsIDs.Contains(userID) && !fieldName.Equals("GuestUsers")) {
                if (GuestUsers > 0)
                {
                    GuestUsers--;
                }
            }
            else
            {
                switch (fieldName)
                {
                    case "GuestUsers":
                        GuestUsers++;
                        break;

                    case "RegisteredUsers":
                        RegisteredUsers++;
                        if (GuestUsers > 0)
                            GuestUsers--;
                        break;

                    case "ManagersNotOwners":
                        ManagersNotOwners++;
                        if (GuestUsers > 0)
                            GuestUsers--;
                        break;


                    case "Owners":
                        Owners++;
                        if (GuestUsers > 0)
                            GuestUsers--;
                        break;


                    case "Admins":
                        Admins++;
                        if (GuestUsers > 0)
                            GuestUsers--;
                        break;
                }
                visitorsIDs.Add(userID);
            }

            //save in DB
            sendStatus();
            String date = today.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            Mapper.getInstance().Update(new DTO_Monitor(date, GuestUsers, RegisteredUsers, ManagersNotOwners, Owners, Admins));
        }

        public void sendStatus()
        {
            String date = today.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            NotificationService.GetInstance().sendMonitorStatus(new DTO_Monitor(date, GuestUsers, RegisteredUsers, ManagersNotOwners, Owners, Admins));
        }
    }
}
