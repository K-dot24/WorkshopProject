﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Terminal3.DomainLayer.StoresAndManagement.Stores;

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
        public ProductSearchAttributes() { }
        public ProductSearchAttributes([OptionalAttribute] String Name,
                                       [OptionalAttribute] String Category,
                                       [OptionalAttribute] List<String> Keywords,
                                       [OptionalAttribute] Double LowPrice,
                                       [OptionalAttribute] Double HighPrice,
                                       [OptionalAttribute] Double ProductRating,
                                       [OptionalAttribute] Double StoreRating)
        {
            this.Name = Name;
            this.Category = Category;
            this.Keywords = Keywords;
            this.LowPrice = LowPrice;
            this.HighPrice = HighPrice;
            this.ProductRating = ProductRating;
            this.StoreRating = StoreRating;
        }
        /// <summary>
        /// Filter out product if its not meet the search criteria
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        internal bool checkProduct(Double StoreRating,Product product)
        {
            Boolean result = true;
            PropertyInfo[] properties = typeof(ProductSearchAttributes).GetProperties();
            for (int i=0; i< properties.Length && result; i++)
            {   var value = properties[i].GetValue(this);
                if (value != null)
                {
                    switch((string)properties[i].Name)
                    { 
                        case "Name":
                            if (!product.Name.Contains((string)value)) {result = false;}
                            break;

                        case "Category":
                            if (!product.Category.Equals((string)value)) { result = false; }
                            break;
                        case "LowPrice":
                            if (product.Price<(Double)value) { result = false; }
                            break;
                        case "HighPrice":
                            if (product.Price > (Double)value) { result = false; }
                            break;
                        case "ProductRating":
                            if (product.Rating < (Double)value) { result = false; }
                            break;
                        case "StoreRating":
                            if (StoreRating < (Double)value) { result = false; }
                            break;
                        case "Keywords":
                            foreach(string keyword in (List<String>)value)
                            {
                                if (product.Keywords.Contains(keyword)) { 
                                    //One keyword has been found
                                    break;
                                }
                            }
                            //No keyword has been found
                            result = false;
                            break;



                    }
                }
            }
            return result;
        }
    }

    public static class ObjectDictionaryMapper<T>
    {
        /// <summary>
        /// Util function to build an object of type T from dictionary
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static T GetObject(IDictionary<String, Object> dict)
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            T res = Activator.CreateInstance<T>();
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].CanWrite && dict.ContainsKey(props[i].Name))
                {
                    props[i].SetValue(res, dict[props[i].Name], null);
                }
            }
            return res;
        }

        /// <summary>
        /// Util function to dump object of type T to dictionary
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<String, Object> GetDictionary(T obj)
        {
            IDictionary<String, Object> res = new Dictionary<String, Object>();
            PropertyInfo[] props = typeof(T).GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].CanRead)
                {
                    res.Add(props[i].Name, props[i].GetValue(obj, null));
                }
            }
            return res;
        }

        /// <summary>
        /// Util function for update several properties of an object using dictionary
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dict"></param>
        public static void SetPropertyValue(Object obj, IDictionary<String, Object> dict)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].CanWrite && dict.ContainsKey(properties[i].Name))
                {
                    properties[i].SetValue(obj, dict[properties[i].Name], null);
                }
            }
        }
    }

}
