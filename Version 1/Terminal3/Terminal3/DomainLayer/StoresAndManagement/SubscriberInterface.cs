using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagement
{
    public interface SubscriberInterface        // TODO - delete ?
    {
        Result<Boolean> Update(Notification notification);
    }
}
