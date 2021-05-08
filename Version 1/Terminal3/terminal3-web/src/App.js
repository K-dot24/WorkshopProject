import React, { useState, useEffect } from 'react';

import { Stores, Navbar, Cart, Checkout, StorePage, Register, Login } from './components';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
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
    const [cart, setCart] = useState({products: [], totalPrice: 0});

    //#region Cart Functionality 
    
    // TODO: Fetch from API?
    const fetchCart = async () => {
    }

    // TODO: Update with real data
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
    

    useEffect(() => {
        fetchCart();
    }, []);

    //printing
    // useEffect(() => {
    //     console.log(cart);
    // }, [cart]);

    return (
        <MuiThemeProvider theme={theme}>
            <Router>
                <div>
                    <Navbar id={-1} totalItems={cart.products.length} />
                    <Switch>
                        <Route exact path="/" component={Stores} />
                            {/* <Stores stores={stores} />
                        </Route> */}

                        <Route path="/stores/:id" render={(props) => (<StorePage handleAddToCart={handleAddToCart} {...props} />)} />

                        <Route exact path="/cart">
                            <Cart
                                id={0} 
                                cart={cart} 
                                handleUpdateCartQuantity={handleUpdateCartQuantity} 
                                handleRemoveFromCart={handleRemoveFromCart} 
                                handleEmptyCart={handleEmptyCart} 
                            />
                        </Route>

                        <Route path="/register" component={Register} />
                        <Route path="/login" component={Login} />

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
