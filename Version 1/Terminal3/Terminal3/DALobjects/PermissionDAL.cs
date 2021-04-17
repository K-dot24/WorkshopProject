using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class PermissionDAL
    {
        //Properties
        public StoreDAL Store { get; }
        public Boolean[] functionsBitMask { get; }

        //Constructor
        public PermissionDAL(StoreDAL store, bool[] functionsBitMask)
        {
            Store = store;
            this.functionsBitMask = functionsBitMask;
        }
    }
}
