using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DALobjects
{
    public class StoreDAL
    {
        //Properties
        public StoreOwnerDAL Founder { get; }
        public LinkedList<StoreOwnerDAL> Owners { get; }
        public LinkedList<StoreManagerDAL> Managers { get; }
        public InventoryManager InventoryManager { get; }   //TODO - do we need a DAL calss of the Inventory Manager?
        public PolicyManager PolicyManager { get; }     //TODO - do we need a DAL calss of the Policy Manager?
        public HistoryDAL History { get; }
        public String StoreID { get; }

        //Constructor
        public StoreDAL(StoreOwnerDAL founder, LinkedList<StoreOwnerDAL> owners, LinkedList<StoreManagerDAL> managers, InventoryManager inventoryManager, PolicyManager policyManager, HistoryDAL history , String ID)
        {
            Founder = founder;
            Owners = owners;
            Managers = managers;
            InventoryManager = inventoryManager;
            PolicyManager = policyManager;
            History = history;
            StoreID = ID;
        }
    }
}
