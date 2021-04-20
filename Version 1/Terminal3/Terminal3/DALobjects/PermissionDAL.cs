using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class PermissionDAL
    {
        //Properties
        public Boolean[] functionsBitMask { get; }
        public bool isOwner { get; }

        //Constructor
        public PermissionDAL(bool[] functionsBitMask, bool isOwner=false)
        {
            this.isOwner = isOwner;
            this.functionsBitMask = functionsBitMask;
        }
    }
}
