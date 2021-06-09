using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Terminal3.DataAccessLayer;
using Terminal3.DataAccessLayer.DTOs;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
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

        private MonitorController()
        {
            DTO_Monitor dto = Mapper.getInstance().LoadMonitor();
            today = DateTime.Now.Date;
            GuestUsers = dto.GuestUsers;
            RegisteredUsers = dto.RegisteredUsers;
            ManagersNotOwners = dto.ManagersNotOwners;
            Owners = dto.Owners;
            Admins = dto.Admins;
        }

        public static MonitorController getInstance()
        {
            if (Instance == null)
            {
                Instance = new MonitorController();
            }
            return Instance;
        }

        public void update(String fieldName)
        {
            if (DateTime.Now.Date > today)
            {
                GuestUsers = 0;
                RegisteredUsers = 0;
                ManagersNotOwners = 0;
                Owners = 0;
                Admins = 0;
            }

            switch (fieldName)
            {
                case "GuestUsers":
                    GuestUsers--;
                    break;

                case "RegisteredUsers":
                    RegisteredUsers++;
                    GuestUsers--;
                    break;

                case "ManagersNotOwners":
                    ManagersNotOwners++;
                    GuestUsers--;
                    break;


                case "Owners":
                    Owners++;
                    GuestUsers--;
                    break;


                case "Admins":
                    Admins++;
                    GuestUsers--;
                    break;
            }

            //save in DB
            String date = today.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo);
            Mapper.getInstance().Update(new DTO_Monitor(date, GuestUsers, RegisteredUsers, ManagersNotOwners, Owners, Admins));
        }
    }
}
