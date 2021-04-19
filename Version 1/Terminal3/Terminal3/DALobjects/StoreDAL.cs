using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace Terminal3.DALobjects
{
    public class StoreDAL
    {
        //Properties       
        public String Id { get; }
        public String Name { get; }
        public String Founder { get; }
        public LinkedList<String> Owners { get; }
        public LinkedList<String> Managers { get; }
        //public InventoryManager InventoryManager { get; }   //TODO - do we need a DAL calss of the Inventory Manager?
        //public PolicyManager PolicyManager { get; }         //TODO - do we need a DAL calss of the Policy Manager?
        public HistoryDAL History { get; }
        public Double Rating { get; private set; }
        public int NumberOfRates { get; private set; }
        
        //Constructor
        public StoreDAL(string id, string name, String founder, LinkedList<String> owners, LinkedList<String> managers, HistoryDAL history, double rating, int numberOfRates)
        {
            Id = id;
            Name = name;
            Founder = founder;
            Owners = owners;
            Managers = managers;
            History = history;
            Rating = rating;
            NumberOfRates = numberOfRates;
        }

        

    }
}
