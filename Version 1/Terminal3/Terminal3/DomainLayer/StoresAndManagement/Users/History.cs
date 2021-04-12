using System.Collections.Generic;

namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public class History
    {
        public LinkedList<ShoppingBag> ShoppingBags { get; }

        public History(LinkedList<ShoppingBag> shoppingBags)
        {
            this.ShoppingBags = shoppingBags;
        }

        public History()
        {
            throw new System.NotImplementedException();
        }
    }
}
