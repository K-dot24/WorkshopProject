import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import { MuiThemeProvider, createMuiTheme } from '@material-ui/core/styles';

import { Stores, Navbar, Cart, Checkout, StorePage, Register, Login, Action } from './components';
import { Register as RegisterAPI, Login as LoginAPI, Logout, OpenNewStore, AddProductToCart, GetUserShoppingCart, UpdateShoppingCart } from './api/API';

// primary and secondary colors for the app
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
    const [user, setUser] = useState({id: -1, email: ''});

    //#region Stores Functionality

    const handleOpenNewStore = async (data) => {
        OpenNewStore({ userID: user.id, ...data }).then(response => response.json().then(json => console.log(json))).catch(err => console.log(err));
    }

    //#endregion

    //#region Cart Functionality 
    
    // TODO: Fetch from API?
    const fetchCart = async () => {
        setCart({products: [], totalPrice: 0});

        // if (user.id !== -1){
        //     // get from API
        // }
    }

    const handleAddToCart = async (storeId, productId, name, price, quantity, image) => {
        AddProductToCart({userID: user.id, productId, ProductQuantity: quantity, storeId}).then(response => response.json().then(json => console.log(json))).catch(err => console.log(err));
        
        
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

    //#region User Functionality

    const handleLogin = async (data) => {
        
        // TODO: What to do in failure?
        LoginAPI(data).then(response => response.ok ? 
            response.json().then(id => setUser({ id, email: data.email})) : null).catch(err => console.log(err));
    }

    // TODO: What to do in success/failure?
    const handleRegister = async (data) => {
        RegisterAPI(data).then(response => response.ok ? 
            response.json().then(id => console.log(id)) : null).catch(err => console.log(err));
    }

    // TODO: What to do in failure?
    const handleLogOut = () => {
        Logout(user.email).then(response => response.ok ?
            response.json().then(message => setUser({id: -1, name: '', email: ''})) : console.log("NOT OK")).catch(err => console.log(err));
    }

    //#endregion

    // Update cart when user change (login/sign out)
    useEffect(() => {
        fetchCart();
        console.log(user);
    }, [user]);

    return (
        <MuiThemeProvider theme={theme}>
            <Router>
                <div>
                    <Navbar storeId={-1} totalItems={cart.products.length} user={user} handleLogOut={handleLogOut} />
                    <Switch>

                        {/* <Route path="/stores/:id" render={(props) => (<StorePage handleAddToCart={handleAddToCart} user={user} handleLogOut={handleLogOut} {...props} />)} /> */}

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
                        
                        <Route exact path={`/${user.id}/openstore`} render={(props) => (<Action name='Open New Store' fields={['Store Name']} handleAction={handleOpenNewStore} {...props} />)} />
                        
                        <Route path="/" render={(props) => (<Stores user={user} handleAddToCart={handleAddToCart} handleLogOut={handleLogOut} {...props} />)} />
                    </Switch>
                </div>
            </Router>
        </MuiThemeProvider>
    )
}

export default App;
