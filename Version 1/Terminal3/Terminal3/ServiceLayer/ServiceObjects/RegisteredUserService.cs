using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.ServiceLayer.ServiceObjects
{
    public class RegisteredUserService : UserService
    {
        //Properties
        public String Id { get; }
        public String Email { get; }
        public Boolean LoggedIn { get; set; }

        //Constructor
        public RegisteredUserService(string userId, string email,bool loggedIn , ShoppingCartService shoppingCartDAL) : base(userId, shoppingCartDAL)
        {
            Id = userId;
            Email = email;
            LoggedIn = loggedIn;
        }        

    }
}
