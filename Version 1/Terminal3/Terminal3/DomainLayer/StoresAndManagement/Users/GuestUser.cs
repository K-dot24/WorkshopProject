using System;
using Terminal3.DALobjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class GuestUser : User
    {
        public Boolean Active { get; }

        public GuestUser() : base()
        {
            Active = true;
        }


        
    }
}
