using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement;
using Terminal3.ServiceLayer;
using Terminal3.ServiceLayer.Controllers;

namespace Terminal3.ServiceLayer
{
    public class Initializer
    {
        ECommerceSystem System;

        public Initializer() { }
        public static void init(StoresAndManagementInterface StoresAndManagement,
             IGuestUserInterface GuestUserInterface, IRegisteredUserInterface RegisteredUserInterface,
            IStoreStaffInterface StoreStaffInterface, SystemAdminController SystemAdminInterface , 
            IDataController DataController , NotificationService NotificationService, HubConnection connection)
        {
            StoresAndManagement = new StoresAndManagementInterface();
            GuestUserInterface = new GuestUserController(StoresAndManagement);
            RegisteredUserInterface = new RegisteredUserController(StoresAndManagement);
            StoreStaffInterface = new StoreStaffController(StoresAndManagement);
            SystemAdminInterface = new SystemAdminController(StoresAndManagement);
            DataController = new DataController(StoresAndManagement);

            string url = "https://localhost:4001/signalr/notification";
            connection = new HubConnectionBuilder()
               .WithUrl(url)
               .WithAutomaticReconnect()
               .Build();
            connection.StartAsync();
            while (connection.State != HubConnectionState.Connected) { }
            NotificationService = NotificationService.GetInstance();
            NotificationService.connection = connection;
        }         

    }
}
