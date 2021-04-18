using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class PermissionDAL
    {
        //Properties
        public Boolean[] functionsBitMask { get; }

        //Constructor
        public PermissionDAL(bool[] functionsBitMask)
        {
            this.functionsBitMask = functionsBitMask;
        }
    }
}
