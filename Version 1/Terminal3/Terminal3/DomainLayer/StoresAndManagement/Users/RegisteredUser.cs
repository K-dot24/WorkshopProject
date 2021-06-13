using System;
using Terminal3.ServiceLayer.ServiceObjects;
using System.Reflection;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Terminal3.ExternalSystems;
using Terminal3.DataAccessLayer;
using MongoDB.Driver;
using MongoDB.Bson;
using Terminal3.DataAccessLayer.DTOs;
using System.Security.Cryptography;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores.Policies.Offer;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class RegisteredUser : User
    {
        //Properties
        public String Email { get; }
        public String Password { get; set; }        
        public Boolean LoggedIn { get; set; }
        public History History { get; set; }
        public LinkedList<Notification> PendingNotification { get; set; }
        public NotificationCenter NotificationCenter { get; set; }


        //public Mapper mapper = Mapper.getInstance();

        //Constructor
        public RegisteredUser(String email , String password) : base()
        {
            this.Email = email;
            var sha1 = new SHA1CryptoServiceProvider();
            var hash_pass = sha1.ComputeHash(Encoding.ASCII.GetBytes(password));

            this.Password = Encoding.ASCII.GetString(hash_pass);
            this.LoggedIn = false;
            this.History = new History();
            this.PendingNotification = new LinkedList<Notification>();
            this.NotificationCenter = NotificationCenter.GetInstance();
        }

        public RegisteredUser(String Id, String email, String password, Boolean loggedin) : base(Id)
        {
            this.Email = email;
            this.Password = password;
            this.LoggedIn = loggedin;
            this.History = new History(); //placeholder
            this.PendingNotification = new LinkedList<Notification>(); //placeholder
            this.NotificationCenter = NotificationCenter.GetInstance();
        }


        // For database load
        public RegisteredUser(String Id , String email, String password , Boolean loggedin , History history , LinkedList<Notification> notifications ) : base(Id)
        {
            this.Email = email;
            this.Password = password;
            this.LoggedIn = loggedin;
            this.History = history;
            this.PendingNotification = notifications;
            this.NotificationCenter = NotificationCenter.GetInstance();
        }


        public RegisteredUser(string id, String email, String password) : base(id)
        {
            this.Email = email;
            var sha1 = new SHA1CryptoServiceProvider();
            var hash_pass = sha1.ComputeHash(Encoding.ASCII.GetBytes(password));

            this.Password = Encoding.ASCII.GetString(hash_pass); this.LoggedIn = false;
            this.History = new History();
            this.PendingNotification = new LinkedList<Notification>();
            this.NotificationCenter = NotificationCenter.GetInstance();
        }

        //Methods
        public Result<RegisteredUser> Login(String password) {
            if (LoggedIn) {
                //User already logged in
                return new Result<RegisteredUser>($"{this.Email} already logged in", false, null);
            }
            var sha1 = new SHA1CryptoServiceProvider();
            var hash_pass = sha1.ComputeHash(Encoding.ASCII.GetBytes(password));

            String hashed_string = Encoding.ASCII.GetString(hash_pass);
            if (this.Password.Equals(hashed_string))
            {
                // Correct paswword
                LoggedIn = true;
                DisplayPendingNotifications();
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

        public Result<Product> AddProductReview(Store store, Product product , String review)
        {
            if (checkIfProductPurchasedByUser(store , product))
            {
                product.AddProductReview(Id , review);
                return new Result<Product>("The product review was added successfuly\n", true, product);
            }
            return new Result<Product>("The User did not purchase the product before, therefore can not write it a review\n", false, null);
        }

        private Boolean checkIfProductPurchasedByUser(Store store, Product product)
        {
            LinkedList<ShoppingBagService> shoppingBags = History.ShoppingBags;
            foreach (ShoppingBagService bag in shoppingBags)
            {             
                foreach(Tuple<ProductService,int> productQuantity in bag.Products)
                {
                    ProductService productInHistory = productQuantity.Item1;
                    if (productInHistory.Id.Equals(product.Id)) { return true; }
                }

            }
            return false;
        }

        public Result<History> GetUserPurchaseHistory()
        {
            return new Result<History>("User history\n", true, History);
        }

        public Result<RegisteredUserService> GetDAL()
        {
            ShoppingCartService SCD = this.ShoppingCart.GetDAL().Data;
            return new Result<RegisteredUserService>("RegisteredUser DAL object" , true , new RegisteredUserService(this.Id, this.Email, this.LoggedIn , SCD));
        }

        public Result<Boolean> ExitSystem()
        {
            Result<GuestUser> res = LogOut();
            if (res.ExecStatus)
                return new Result<Boolean>("User exit successfuly", true, true);
            else
                return new Result<Boolean>("Can not exit", false, false);

        }

        public new Result<ShoppingCart> Purchase(IDictionary<String, Object> paymentDetails, IDictionary<String, Object> deliveryDetails, MongoDB.Driver.IClientSessionHandle session = null)
        {
            if (ShoppingCart.ShoppingBags.IsEmpty)
            {
                return new Result<ShoppingCart>("The shopping cart is empty\n", false, null);
            }

            Result<ShoppingCart> result = ShoppingCart.Purchase(paymentDetails, deliveryDetails, AcceptedOffers, session);
            if (result.Data != null)
            {
                History.AddPurchasedShoppingCart(ShoppingCart, session);
                this.ShoppingCart = new ShoppingCart();          // create new shopping cart for user

                /* // Update DB
                 var filter = Builders<BsonDocument>.Filter.Eq("_id", this.Id);
                 var update = Builders<BsonDocument>.Update.Set("ShoppingCart", ShoppingCart.getDTO());
                 mapper.UpdateRegisteredUser(filter, update);*/

                Result<bool> removeAccatedOffersResult = removeAcceptedOffers();
                if (!removeAccatedOffersResult.ExecStatus)
                    return new Result<ShoppingCart>("The purchase failed because the system failed to remove accepted offers", false, null);

            }
            return result;
        }
    
        public Result<Boolean> Update(Notification notification, MongoDB.Driver.IClientSessionHandle session = null)
        {
            if (LoggedIn)
            {
                NotificationCenter.notifyNotificationServer(notification);
                return new Result<Boolean>("User is LoggedIn , therefor displaying the notification\n", true, true);
            }
            PendingNotification.AddLast(notification);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", Id);
            var update_notification = Builders<BsonDocument>.Update.Set("PendingNotification", getPendingNotificationsDTO());
            Mapper.getInstance().UpdateRegisteredUser(filter, update_notification , session:session);
            return new Result<Boolean>("User not logged in , therefore the notification is added to pending list\n", false, false);
        }    

        private void DisplayPendingNotifications()
        {
            foreach(Notification notification in PendingNotification)
            {
                if (!notification.isOpened)
                {
                    NotificationCenter.notifyNotificationServer(notification);
                }
            }

            RemoveOpenedNotifications();
        }

        private void RemoveOpenedNotifications()
        {
            List<Notification> pendings = new List<Notification>(PendingNotification);
            for(int i=0;i< pendings.Count;i++)
            {
                Notification notification = pendings[i];
                if (notification.isOpened)
                {
                    PendingNotification.Remove(notification);
                }
            }
            var filter = Builders<BsonDocument>.Filter.Eq("_id", Id);
            var update_notification = Builders<BsonDocument>.Update.Set("PendingNotification", getPendingNotificationsDTO());
            Mapper.getInstance().UpdateRegisteredUser(filter, update_notification);
            
        }

        public DTO_RegisteredUser getDTO()
        {            
            return new DTO_RegisteredUser(Id, ShoppingCart.getDTO(), Email, Password, 
                                        LoggedIn, History.getDTO(), getPendingNotificationsDTO() , Get_DTO_Offers() );
        }

        public LinkedList<DTO_Notification> getPendingNotificationsDTO()
        {
            LinkedList<DTO_Notification> notifications_dto = new LinkedList<DTO_Notification>();
            foreach (var n in PendingNotification)
            {
                notifications_dto.AddLast(n.getDTO());
            }
            return notifications_dto;
        }

        public override Result<bool> AcceptOffer(string offerID)
        {
            Offer offer = findPendingOffer(offerID);
            Result<bool> res = MovePendingOfferToAccepted(offerID);
            if (!res.ExecStatus)
                return res;
            return NotificationCenter.notifyOfferRecievedUser(this.Id, offer.StoreID, offer.ProductID, offer.Amount, offer.Price, offer.CounterOffer, true);
        }

        public override Result<bool> DeclineOffer(string offerID)
        {
            Offer offer = findPendingOffer(offerID);
            RemovePendingOffer(offerID);
            return NotificationCenter.notifyOfferRecievedUser(this.Id, offer.StoreID, offer.ProductID, offer.Amount, offer.Price, offer.CounterOffer, false);
        }

        public override Result<bool> CounterOffer(string offerID)
        {
            Offer offer = findPendingOffer(offerID);
            return NotificationCenter.notifyOfferRecievedUser(this.Id, offer.StoreID, offer.ProductID, offer.Amount, offer.Price, offer.CounterOffer, false);
        }
    }
}
