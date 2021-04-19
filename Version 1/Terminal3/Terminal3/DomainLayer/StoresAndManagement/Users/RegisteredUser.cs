using System;
using Terminal3.DALobjects;
using System.Reflection;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.ExternalSystems;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class RegisteredUser : User
    {
        //Properties
        public String Email { get; }
        public String Password { get; }
        public Boolean LoggedIn { get; set; }
        public History History { get; set; }

        //Constructor
        public RegisteredUser(String email , String password) : base()
        {
            this.Email = email;
            this.Password = password;
            this.LoggedIn = false;
            this.History = new History();
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
        public Result<GuestUser> LogOut() {
            if (!LoggedIn)
            {
                //User already logged out
                return new Result<GuestUser>($"{this.Email} already logged out", false, null);
            }
            else 
            {
                LoggedIn = false;
                return new Result<GuestUser>($"{this.Email} is Logged out\n", true, new GuestUser());
            }           
        }

        public Result<Boolean> AddProductReview(Store store, Product product , String review)
        {
            if (checkIfProductPurchasedByUser(store , product))
            {
                product.AddProductReview(Id , review);
                return new Result<Boolean>("The product review was added successfuly\n", true, true);
            }
            return new Result<Boolean>("The User did not purchase the product before, therefore can not write it a review\n", false, false);
        }

        private Boolean checkIfProductPurchasedByUser(Store store, Product product)
        {
            LinkedList<ShoppingBagDAL> shoppingBags = History.ShoppingBags;
            foreach (ShoppingBagDAL bag in shoppingBags)
            {                 
                if (bag.Products.ContainsKey(product.GetDAL().Data))
                {
                    return true;
                }
            }
            return false;
        }

        public Result<History> GetUserPurchaseHistory()
        {
            return new Result<History>("User history\n", true, History);
        }
        public Result<RegisteredUserDAL> GetDAL()
        {
            ShoppingCartDAL SCD = this.ShoppingCart.GetDAL().Data;
            return new Result<RegisteredUserDAL>("RegisteredUser DAL object" , true , new RegisteredUserDAL(this.Id, this.Email, this.Password, this.LoggedIn , SCD));
        }

        public Result<Boolean> ExitSystem()
        {
            Result<GuestUser> res = LogOut();
            if (res.ExecStatus)
                return new Result<Boolean>("User exit successfuly", true, true);
            else
                return new Result<Boolean>("Can not exit", false, false);

        }

        public Result<ShoppingCart> Purchase(IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails)
        {
            Double amount = ShoppingCart.GetTotalShoppingCartPrice();

            bool paymentSuccess = PaymentSystem.Pay(amount, paymentDetails);
            bool deliverySuccess = DeliverySystem.Deliver(deliveryDetails);

            if (!paymentSuccess)
            {
                return new Result<ShoppingCart>("Atempt to purchase the shopping cart faild due to error in payment details\n", false, null);

            }
            if (!deliverySuccess)
            {
                return new Result<ShoppingCart>("Atempt to purchase the shopping cart faild due to error in delivery details\n", false, null);
            }

            History.AddPurchasedShoppingCart(ShoppingCart);
            ShoppingCart copy = new ShoppingCart(ShoppingCart);
            ShoppingCart = new ShoppingCart();          // create new shopping cart for user

            return new Result<ShoppingCart>("Users purchased shopping cart\n", true, copy);
        }
    }
}
