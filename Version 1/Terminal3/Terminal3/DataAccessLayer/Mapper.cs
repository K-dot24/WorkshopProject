/*using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using Terminal3.DALobjects;
using Terminal3.DomainLayer.StoresAndManagement.Users;

namespace Terminal3.DataAccessLayer
{
    public class Mapper
    {
        //Fields
        private static Mapper instance = null;
        public ConcurrentDictionary<String, History> Historys { get; }


        //Constructor
        private Mapper()
        {
         
        }

        public static Mapper getInstance()
        {
            if (instance == null)
            {
                instance = new Mapper();
            }
            return instance;
        }

        #region History
        /*public void create(History history)
        {
            Historys.TryAdd(history.UserID, history);
        }*/

        /*public void update(History history)
        {
            Historys.AddOrUpdate(history.UserID, history, (oldkey, oldvalue) => history);
        }

        public void delete(History history)
        {
            History _history;
            Historys.Remove(history.UserID , out _history);
        }*/

        //public History get(String UserID)
        //{
        //    //TODO
        //}
        //public History get(HistoryDAL historyDAL)
        //{
        //    //TODO
        //}

//        #endregion


//    }
//}
//*/