using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer
{
    public static class Service
    {
        public static String GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }        
        
    }

    public class Result<T>
    {
        public String Message { get; set; }
        public Boolean ExecStatus { get; set; }
        public T Data { get; set; }      

        public Result(string message, bool execStatus, T data)
        {
            this.Message = message;
            this.ExecStatus = execStatus;
            this.Data = data;
        }

    }
}
