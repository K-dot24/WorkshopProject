using System;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public interface IStoreStaff
    {
        Result<object> GetDAL();
        String GetId();
    }
}
