using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class PermissionDAL
    {
        public StoreDAL Store { get; }
        public Boolean[] functionsBitMask { get; }

        public PermissionDAL(StoreDAL store, bool[] functionsBitMask)
        {
            Store = store;
            this.functionsBitMask = functionsBitMask;
        }
    }
}
