using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ServiceLayer.ServiceObjects
{
    public class IStoreStaffService
    {
        public String Id { get; }

        public IStoreStaffService(string id)
        {
            Id = id;
        }
    }

}
