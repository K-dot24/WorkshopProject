using signalRgateway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace signalRgateway.Client
{
    public interface IClient {
        Task ReceiveMessage(String message);

    }
}
