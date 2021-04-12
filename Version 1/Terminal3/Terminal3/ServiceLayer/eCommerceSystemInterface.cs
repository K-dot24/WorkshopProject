using System;
using System.Collections.Generic;
using System.Text;
using Terminal3.DomainLayer;
using Terminal3.DomainLayer.StoresAndManagment.Users;
using Terminal3.DomainLayer.StoresAndManagment.Stores;

namespace Terminal3.ServiceLayer
{
    public interface eCommerceSystemInterface
    {
        Result<Boolean> ResetSystem();
        Result<Boolean> Resiter(String email , String password);         
        Result<Boolean> Login(String email, String password);
        Result<Boolean> LogOut(String email, String password);
        Result<Boolean> SearchStore(SearchAttributes details);
        Result<Boolean> SearchProduct(SearchAttributes details);
        Result<Boolean> AddProductToCart(User user, Product product);   // Redundent ?
        Result<Boolean> GetUserShoppingCart(User user);
        Result<Boolean> UpdateShoppingCart(User user , Product product , int quantity);
        Result<Boolean> Purchase(User user);
        Result<Boolean> GetTotalShoppingCartPrice(User user);


    }
}
