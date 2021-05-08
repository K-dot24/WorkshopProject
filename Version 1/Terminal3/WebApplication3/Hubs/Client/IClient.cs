using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal3.DomainLayer.StoresAndManagement;

namespace Terminal3WebAPI.Hubs.Client
{
    public interface IClient { 
        Task RecieveNotification(Notification message);

    }
}
