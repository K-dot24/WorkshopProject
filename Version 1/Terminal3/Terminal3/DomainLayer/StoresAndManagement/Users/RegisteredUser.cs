using System;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class RegisteredUser : User
    {
        public string UserId { get; }
        public string Email { get; }
        public string Password { get; }


        public RegisteredUser(String email , String password)
        {
            this.UserId = Service.GenerateId();
            this.Email = email;
            this.Password = password;
        } 
    }
}
