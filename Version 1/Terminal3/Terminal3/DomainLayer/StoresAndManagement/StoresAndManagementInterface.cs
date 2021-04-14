using System;
using System.Collections.Generic;
using System.Text;
namespace Terminal3.DomainLayer.StoresAndManagement
{
    public interface IStoresAndManagementInterface
    {
        Result<Object> Register(String email, String password);
    }
    public class StoresAndManagementInterface : IStoresAndManagementInterface
    {
        public Result<Object> Register(String email, String password)
        {
            throw new NotImplementedException();
        }
    }
}
