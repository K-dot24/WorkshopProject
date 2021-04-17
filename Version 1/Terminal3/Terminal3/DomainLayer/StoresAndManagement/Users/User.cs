using Terminal3.DALobjects;


namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public abstract class User
    {
        public ShoppingCart ShoppingCart { get; }
        //TODO - unique field 

        protected User()
        {
            ShoppingCart = new ShoppingCart();
        }

        //TODO: Fix DAL
        /*protected User(UserDAL userDAL)
        {
            ShoppingCart = new ShoppingCart(userDAL.ShoppingCart);
        }*/

    }
}
