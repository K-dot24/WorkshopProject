﻿using System;

namespace Terminal3.DomainLayer.StoresAndManagement.Stores
{
    public class Product
    {
        public String Id { get; }
        public String Name { get; }
        public Double Price { get; }
        public int Quantity { get; }

        public Product(string name, double price, int quantity)
        {
            Id = Service.GenerateId();
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        //TODO: functions?
    }
}
