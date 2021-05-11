import React, { useState, useEffect, useCallback } from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import { MuiThemeProvider, createMuiTheme } from '@material-ui/core/styles';

// SignalR
import { HubConnectionBuilder } from '@microsoft/signalr';

import { Stores, Navbar, Cart, Checkout, Register, Login, Action } from './components';
import { Register as RegisterAPI, Login as LoginAPI, Logout, OpenNewStore, AddProductToCart, 
        GetUserShoppingCart, UpdateShoppingCart, EnterSystem } from './api/API';

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
    const [user, setUser] = useState({id: -1, email: '', loggedIn: false});
    const [cart, setCart] = useState({id: 0, products: [], totalPrice: 0});

    // SignalR
    const [connection, setConnection] = useState(null);

    const handleEnterSystem = async () => {
        EnterSystem().then(response => response.ok ? 
            response.json().then(id => setUser({ ...user, id }) ) : console.log("NOT OKAY")).catch(err => alert(err));
    }

    //#region Cart Functionality 
    
    // TODO: Fetch from API?
    const fetchCart = async () => {
        setCart({id: 0, products: [], totalPrice: 0});

        if (user.loggedIn === true){
            GetUserShoppingCart(user.id).then(response => response.ok ? 
                response.json().then(data => setCart({id: data.id,
                                                      products: data.shoppingBags.length === 0 ? [] : 
                                                        data.shoppingBags.reduce(function(list, bag) {
                                                            return list.concat(bag.products);
                                                        }, []),  
                                                      totalPrice: data.shoppingBags.reduce(function(total, bag) {
                                                                    return total + bag.totalBagPrice;
                                                                }, 0)
                                                        })) : null).catch(err => console.log(err));    // TODO: Check
        }
    }

    const handleAddToCart = async (storeId, productId, name, price, quantity, image) => {
        AddProductToCart({ userID: user.id, productID: productId, ProductQuantity: quantity, storeID: storeId }).then(response => response.ok ? 
            response.json().then(status => alert("Product added successfully")) : console.log("NOT OKAY")).catch(err => alert(err));

        fetchCart();
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
        LoginAPI({ ...data, guestuserid: user.id }).then(response => response.ok ? 
            response.json().then(id => 
                reIdentify(user.id, id, 'Login')
                .then(() => setUser({ id, email: data.email, loggedIn: true}))
                .catch(err => alert(err))) : console.log("NOT OK")).catch(err => alert(err));
    }

    const handleRegister = async (data) => {
        RegisterAPI(data).then(response => response.ok ? 
            response.json().then(id => console.log(id)) : console.log("NOT OK")).catch(err => alert(err));
    }

    const handleLogOut = () => {
        Logout(user.email).then(response => response.ok ?
            response.json().then(id => 
                reIdentify(user.id, id, 'Logout')
                .then(() => setUser({id, email: '', loggedIn: false}))
                .catch(err => alert(err))) : console.log("NOT OK")).catch(err => alert(err));
    }
    
    const handleOpenNewStore = async (data) => {
        OpenNewStore({ userID: user.id, ...data }).then(response => response.json().then(message => alert(message))).catch(err => alert(err));
    }

    // TODO
    const handleAddSystemAdmin = async (data) => {
        console.log(data);
    }
    //#endregion

    //#region Signal-R Functionality
    
    //method can be 'Login' | 'Logout'
    const reIdentify = async (_oldUserID, _newUserID,method)=>{
        const SignalRIreIdentifyModel = {
            oldUserID: _oldUserID,
            newUserID: _newUserID
        }
        if (connection.connectionStarted) {
            try {
                await connection.send(method, SignalRIreIdentifyModel);
            }
            catch(e) {
                console.log(e);
            }
        }
        else {
            alert('No connection to server yet.');
        }
    }

    //#endregion


    // Update cart when user change (login/sign out)
    useEffect(() => {
        fetchCart();
        console.log(user);
    }, [user]);

    useEffect(() => {
        console.log(cart.products);
    }, [cart]);

    useEffect(() => {
        handleEnterSystem();

        // SignalR - Create new connection
        console.log('Creating new connection');
        var newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:4001/signalr/notification')
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
        console.log('after connection');
    }, []);

    // SignalR
    useEffect(() => {
        const IdentifyInServer = (_userid)=> {
            try{
                //Identify in the server
                console.log('before identify');
                const identifer = {
                    userID: _userid
                };
                connection.send('Identify',identifer);
                console.log('after identify');
            }
            catch(e){
                console.log(e);
            }
        }
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');
                    //Register client to listen on ReceiveMessage events
                    connection.on('ReceiveMessage', notification => {
                        //Add functunality when new notification is arrive
                        // console.log('recevice new message')
                        // console.log(notification)
                        alert(notification);
                        // TODO: something
                        // const updatedChat = [...latestChat.current];
                        // updatedChat.push(notification);
                        // setChat(notification);
                    });
                    IdentifyInServer(user.id);
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);


    return (
        <MuiThemeProvider theme={theme}>
            <Router>
                <div>
                    <Navbar storeId={-1} totalItems={cart.products.length} user={user} handleLogOut={handleLogOut} />
                    <Switch>

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
                            <Checkout userID={user.id} cart={cart} handleEmptyCart={handleEmptyCart} />
                        </Route>
                        
                        <Route exact path={`/${user.id}/openstore`} 
                                render={(props) => (<Action name='Open New Store' fields={[{name: 'Store Name', required: true}]} 
                                                            handleAction={handleOpenNewStore} {...props} />)} 
                        />
                        
                        {/* {user.id === "-777" && (
                            <Route exact path={`/${user.id}/addsystemadmin`} 
                                    render={(props) => (<Action name='Add System Admin' fields={[{name: 'Email', required: true}]} 
                                    handleAction={handleAddSystemAdmin} {...props} />)} 
                            />
                        ) } */}
                        
                        
                        <Route path="/" render={(props) => (<Stores user={user} handleAddToCart={handleAddToCart} handleLogOut={handleLogOut} {...props} />)} />

                        
                    </Switch>
                </div>
            </Router>
        </MuiThemeProvider>
    )
}

export default App;
