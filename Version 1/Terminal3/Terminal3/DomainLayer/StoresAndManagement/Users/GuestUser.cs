using System;
using Terminal3.DALobjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class GuestUser : User
    {
        public Boolean Active { get; set; }

        public GuestUser() : base()
        {
            Active = true;
        }

        public Result<Boolean> ExitSystem()
        {
            Active = false;
            return new Result<bool>("User exit system", true, true);
        }


        
    }
}
