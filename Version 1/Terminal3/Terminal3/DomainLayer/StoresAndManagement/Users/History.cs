using System.Collections.Generic;
using Terminal3.DALobjects;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class History
    {
        public LinkedList<ShoppingBag> ShoppingBags { get; }

        public History()
        {
            ShoppingBags = new LinkedList<ShoppingBag>();
        }

        public History(LinkedList<ShoppingBag> shoppingBags)
        {
            this.ShoppingBags = shoppingBags;
        }

        //TODO: Fix DAL

        /*public History(HistoryDAL historyDAL)
        {
            LinkedList<ShoppingBag> shoppingBags = new LinkedList<ShoppingBag>();
            foreach(ShoppingBagDAL sb in historyDAL.ShoppingBags){
                shoppingBags.AddLast(new ShoppingBag(sb));
            }
            this.ShoppingBags = shoppingBags;
        }

        public Result<HistoryDAL> GetDAL()
        {
            LinkedList<ShoppingBagDAL> SBD = new LinkedList<ShoppingBagDAL>();
            foreach(ShoppingBag sb in ShoppingBags)
            {
                SBD.AddLast(sb.GetDAL().Data);
            }

            return new Result<HistoryDAL>("History DAL object", true, new HistoryDAL(SBD));
        }

        public Result<HistoryDAL> GetDAL()
        {
            LinkedList<ShoppingBagDAL> SBD = new LinkedList<ShoppingBagDAL>();
            foreach(ShoppingBag sb in ShoppingBags)
            {
                SBD.AddLast(sb.GetDAL().Data);
            }

            return new Result<HistoryDAL>("History DAL object", true, new HistoryDAL(SBD));
        }*/
    }
}
