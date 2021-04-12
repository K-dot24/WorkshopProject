namespace Terminal3.DomainLayer.StoresAndManagement.Users
{
    public abstract class User
    {
        public ShoppingCart ShoppingCart { get; }

        protected User()
        {
            ShoppingCart = new ShoppingCart();
        }
        
    }
}
