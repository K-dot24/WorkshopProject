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

        protected Result<UserDAL> GetDAL()
        {
            ShoppingCartDAL shoppingCart = ShoppingCart.GetDAL().Data;
            return new Result<UserDAL>("User DAL object", true, new UserDAL(shoppingCart));
        }

    }
}
