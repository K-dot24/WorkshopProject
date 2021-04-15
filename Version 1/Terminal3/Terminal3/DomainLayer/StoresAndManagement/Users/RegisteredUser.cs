using System;

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
    }
}
