using System.Collections.Generic;
using Terminal3.DomainLayer.StoresAndManagement.Stores;
using Terminal3.ServiceLayer.ServiceObjects;
using System;
using Terminal3.DataAccessLayer.DTOs;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class StoreOwner : IStoreStaff
    {
        public RegisteredUser User { get; }
        public Store Store { get; }
        public IStoreStaff AppointedBy { get; set; }
        public LinkedList<StoreManager> StoreManagers { get; }
        public LinkedList<StoreOwner> StoreOwners { get; }

        public StoreOwner(RegisteredUser user, Store store, IStoreStaff appointedBy)
        {
            User = user;
            Store = store;
            AppointedBy = appointedBy;
            StoreManagers = new LinkedList<StoreManager>();
            StoreOwners = new LinkedList<StoreOwner>();
        }

        public Result<object> GetDAL()
        {           
            LinkedList<String> storeOwners = new LinkedList<String>();
            LinkedList<String> storeManagers = new LinkedList<String>();

            foreach (StoreOwner so in StoreOwners)
            {
                storeOwners.AddLast(so.User.Id);
            }

            foreach (StoreManager sm in StoreManagers)
            {
                storeManagers.AddLast(sm.User.Id);
            }
            if(AppointedBy != null)
                return new Result<object>("Store owner DAL object", true, new StoreOwnerService(User.Id, Store.Id, AppointedBy.GetId(), storeOwners, storeManagers));
            return new Result<object>("Store owner DAL object", true, new StoreOwnerService(User.Id, Store.Id, null, storeOwners, storeManagers));
        }

        public string GetId()
        {
            return User.Id;
        }

        public Result<Boolean> Update(Notification notification, MongoDB.Driver.IClientSessionHandle session = null)
        {
            return User.Update(notification , session);
        }
        public DTO_StoreOwner getDTO()
        {
            LinkedList<string> managers_dto = new LinkedList<string>();
            foreach(StoreManager sm in StoreManagers)
            {
                managers_dto.AddLast(sm.GetId());
            }

            LinkedList<string> owners_dto = new LinkedList<string>();
            foreach (StoreOwner so in StoreOwners)
            {
                owners_dto.AddLast(so.GetId());
            }
            if(AppointedBy is null) 
            {
                return new DTO_StoreOwner(User.Id, Store.Id, "", managers_dto, owners_dto);

            }
            else
            {
                return new DTO_StoreOwner(User.Id, Store.Id, AppointedBy.GetId(), managers_dto, owners_dto);

            }
        }
    }
}
