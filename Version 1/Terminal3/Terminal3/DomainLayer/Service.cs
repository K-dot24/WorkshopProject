using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer
{
    public static class Service
    {
        public static String GenerateID()
        {
            return Guid.NewGuid().ToString("N");
        }        
        
    }

    public class Result<T>
    {
        public String message { get; set; }
        public Boolean execStatus { get; set; }
        public T data { get; set; }      

        public Result(string message, bool execStatus, T data)
        {
            this.message = message;
            this.execStatus = execStatus;
            this.data = data;
        }

    }

    public class SearchAttributes
    {

    }
}
