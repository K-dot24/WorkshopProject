using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer.StoresAndManagment.Users
{
    class RegisteredUser : User
    {
        private String userID;
        private String email;
        private String password;


        public RegisteredUser(String email , String password) : base()
        {
            this.userID = Service.GenerateID();
            this.email = email;
            this.password = password;
        } 
    }
}
