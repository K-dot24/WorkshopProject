import React, { useState, useEffect } from 'react';

import { Stores, Navbar, Cart, Checkout, StorePage, Register, Login } from './components';
import { BrowserRouter as Router, Switch, Route, useLocation } from 'react-router-dom';
import { MuiThemeProvider, createMuiTheme } from '@material-ui/core/styles';

const theme = createMuiTheme({
    palette: {
        primary: {
            main: '#00A699'
        },
        secondary: {
        main: '#FF5A5F'
        }
    }
  });

const App = () => {
    // states
    const [cart, setCart] = useState({products: [], totalPrice: 0});
    const [user, setUser] = useState({id: -1, name: '', email: ''});

    //#region Cart Functionality 
    
    // TODO: Fetch from API?
    const fetchCart = async () => {
        setCart({products: [], totalPrice: 0});

        // if (user.id !== -1){
        //     // get from API
        // }
    }

    const handleAddToCart = async (productId, name, price, quantity, image) => {
         setCart(function(prevState) {
            const productArr = prevState.products.filter(p => p.id === productId);
            
            return {
                products: productArr.length === 0 ? [...prevState.products, {id: productId, name, price, quantity, image}] : handleUpdateCartQuantity(productId, productArr[0].quantity + quantity),
                totalPrice: prevState.totalPrice + price*quantity      
            }
        }); 
    }

    const handleUpdateCartQuantity = async (productId, quantity) => {
        if (quantity === 0) {
            handleRemoveFromCart(productId);
        } else {
            setCart(function(prevState) {
                const product = prevState.products.filter(product => product.id === productId)[0];
                const toAdd = quantity - product.quantity;
                
                return {
                    products: prevState.products.map(
                    p => p.id === productId ? { ...p, quantity: quantity }: p
                    ),
                    totalPrice: prevState.totalPrice + product.price * toAdd       
                }
            }); 

            // setCart(prevState => ({
            //     products: prevState.products.map(
            //       product => product.id === productId ? { ...product, quantity: quantity }: product
            //     ),
            //     totalPrice: prevState.totalPrice + prevState.products.filter(product => product.id === productId)[0].price * quantity       
            // })); 
        }
    }

    const handleRemoveFromCart = async (productId) => {

        // setCart(prevState => ({
        //     products: prevState.products.filter(
        //       product => product.id !== productId
        //     ),
        //     totalPrice: prevState.totalPrice - price
        //   }));

        setCart(function(prevState) {
            const product = prevState.products.filter(product => product.id === productId)[0];
            
            return {
            products: prevState.products.filter(
                product => product.id !== productId
            ),
            totalPrice: prevState.totalPrice - product.price * product.quantity       
            }
        }); 
    }

    const handleEmptyCart = async () => {
        setCart({products: [], totalPrice: 0});
    }
    //#endregion

    // TODO: API
    const handleLogin = async (userLoginData) => {
        // check with DB...

        // User exists - what details we need here?
        setUser({ id: 10, email: userLoginData.email });
    }

    // TODO: API
    const handleRegister = async (userRegisterData) => {
        console.log(userRegisterData);
    }

    const handleLogOut = () => {
        setUser({id: -1, name: '', email: ''});
    }

    // Update cart when user change (login/sign out)
    useEffect(() => {
        fetchCart();
    }, [user]);

    return (
        <MuiThemeProvider theme={theme}>
            <Router>
                <div>
                    <Navbar id={-1} totalItems={cart.products.length} user={user} handleLogOut={handleLogOut} />
                    <Switch>
                        <Route exact path="/" component={Stores} />
                            {/* <Stores stores={stores} />
                        </Route> */}

                        <Route path="/stores/:id" render={(props) => (<StorePage handleAddToCart={handleAddToCart} user={user} handleLogOut={handleLogOut} {...props} />)} />

                        <Route exact path="/cart">
                            <Cart
                                id={0} 
                                cart={cart} 
                                handleUpdateCartQuantity={handleUpdateCartQuantity} 
                                handleRemoveFromCart={handleRemoveFromCart} 
                                handleEmptyCart={handleEmptyCart} 
                            />
                        </Route>

                        <Route path="/register" render={(props) => (<Register handleRegister={handleRegister} {...props} />)} />
                        
                        <Route path="/login" render={(props) => (<Login handleLogin={handleLogin} {...props} />)} />

                        <Route exact path="/checkout">
                            <Checkout cart={cart} handleEmptyCart={handleEmptyCart} />
                        </Route>
                    </Switch>
                </div>
            </Router>
        </MuiThemeProvider>
    )
}

export default App;
