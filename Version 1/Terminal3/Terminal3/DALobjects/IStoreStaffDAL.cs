using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class IStoreStaffDAL
    {
        public String Id { get; }

        public IStoreStaffDAL(string id)
        {
            Id = id;
        }
    }

}
