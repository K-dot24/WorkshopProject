using System;
using Terminal3.DataAccessLayer.DTOs;
using Terminal3.ServiceLayer.ServiceObjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class GuestUser : User
    {
        public Boolean Active { get; set; }

        public GuestUser() : base()
        {
            Active = true;
        }

        public GuestUser(String ID , ShoppingCart shoppingCart , bool active) : base(ID , shoppingCart)
        {
            Active = active;
        }
        public GuestUser(String ID, bool active) : base(ID)
        {
            Active = active;
        }        

        public Result<Boolean> ExitSystem()
        {
            Active = false;
            return new Result<bool>("User exit system", true, true);
        }

        public DTO_GuestUser getDTO()
        {
            return new DTO_GuestUser(Id, ShoppingCart.getDTO(), Active);
        }

        public override Result<bool> AcceptOffer(string offerID)
        {
            return MovePendingOfferToAccepted(offerID);
        }

        public override Result<bool> DeclineOffer(string offerID)
        {
            RemovePendingOffer(offerID);
            return new Result<bool>("", true, true);
        }

        public override Result<bool> CounterOffer(string offerID)
        {
            return new Result<bool>("", true, true);
        }
    }
}
