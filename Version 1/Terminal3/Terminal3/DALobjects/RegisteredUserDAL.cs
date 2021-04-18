using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class RegisteredUserDAL : UserDAL
    {
        //Properties
        public String Id { get; }
        public String Email { get; }
        public String Password { get; }
        public Boolean LoggedIn { get; set; }

        //Constructor
        public RegisteredUserDAL(string userId, string email, string password, bool loggedIn , ShoppingCartDAL shoppingCartDAL) : base(userId, shoppingCartDAL)
        {
            Id = userId;
            Email = email;
            Password = password;
            LoggedIn = loggedIn;
        }        

    }
}
