using SignalRgateway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalRgateway.Hubs.Client
{
    public interface IClient {
        Task ReceiveMessage(Notification message);

    }
}
