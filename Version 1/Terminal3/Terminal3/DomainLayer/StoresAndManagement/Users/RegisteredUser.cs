using System;
using Terminal3.DALobjects;
using System.Reflection;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class RegisteredUser : User
    {
        //Properties
        public String UserId { get; }
        public String Email { get; }
        public String Password { get; }
        public Boolean LoggedIn { get; set; }

        //Constructor
        public RegisteredUser(String email , String password)
        {
            this.UserId = Service.GenerateId();
            this.Email = email;
            this.Password = password;
            this.LoggedIn = false;
        }

        public RegisteredUser(RegisteredUserDAL registeredUserDAL)
        {
            this.UserId = registeredUserDAL.UserId;
            this.Email = registeredUserDAL.Email;
            this.Password = registeredUserDAL.Password;
            this.LoggedIn = registeredUserDAL.LoggedIn;
        }

        //Methods
        public Result<RegisteredUser> Login(String password) {
            if (LoggedIn) {
                //User already logged in
                return new Result<RegisteredUser>($"{this.Email} already logged in", false, null);
            }
            if (Password.Equals(password))
            {
                //Correct paswword
                LoggedIn = true;
                return new Result<RegisteredUser>($"{this.Email} is Logged in\n", true, this);
            }
            else
            {
                //Incorrect password
                return new Result<RegisteredUser>($"Incorrect password provided for {this.Email}\n", false, null);

            }
        }
        public Result<RegisteredUser> LogOut() {
            if (!LoggedIn)
            {
                //User already logged out
                return new Result<RegisteredUser>($"{this.Email} already logged out", false, null);
            }
            else 
            {
                LoggedIn = false;
                return new Result<RegisteredUser>($"{this.Email} is Logged out\n", true, this);
            }           
        }

        public Result<RegisteredUserDAL> GetDAL()
        {
            ShoppingCartDAL SCD = this.ShoppingCart.GetDAL().Data;
            return new Result<RegisteredUserDAL>("RegisteredUser DAL object" , true , new RegisteredUserDAL(this.UserId, this.Email, this.Password, this.LoggedIn , SCD));
        }
    }
}
