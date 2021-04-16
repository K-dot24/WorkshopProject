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

        public Result(bool execStatus) //for proxy implementation 
        {
            this.Message = null;
            this.ExecStatus = execStatus;
            this.Data = default;
        }
    }

    public class ProductSearchAttributes
    {

        //Properties
        public String Name { get; set; }
        public String Category { get; set; }
        public Double LowPrice { get; set; }
        public Double HighPrice { get; set; }
        public Double ProductRating { get; set; }
        public Double StoreRating { get; set; }
        public List<String> Keywords { get; set; }

        //Constructor
        public ProductSearchAttributes(String name = "", String category = "",
                                        List<String> keywords = null,
                                        Double? lowPrice,
                                        Double? highPrice,
                                       Double? productRating,
                                       Double? storeRating)
        {
            Name = name;
            Category = category;
            Keywords = keywords;
        }
    }

    }
