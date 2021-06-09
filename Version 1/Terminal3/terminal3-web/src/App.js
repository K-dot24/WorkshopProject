import React, { useState, useEffect, useCallback } from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import { MuiThemeProvider, createMuiTheme } from '@material-ui/core/styles';

// SignalR
import { HubConnectionBuilder } from '@microsoft/signalr';

import { Stores, Navbar, Cart, Checkout, Register, Login, Action, Products, Review } from './components';
import { Register as RegisterAPI, Login as LoginAPI, Logout, OpenNewStore, AddProductToCart, 
        GetUserShoppingCart, UpdateShoppingCart, EnterSystem, SearchProduct, GetTotalShoppingCartPrice,
        GetUserPurchaseHistory, AddSystemAdmin, RemoveSystemAdmin, printErrorMessage, ResetSystem } from './api/API';

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
    const [systemAdmins, setSystemAdmins] = useState(['-777']);
    const [storeSearchQuery, setStoreSearchQuery] = useState('');
    
    // Products Search
    const [productSearchQuery, setProductSearchQuery] = useState('');
    const [products, setProducts] = useState([]);
    
    // User's Purchase History
    const [userPurchaseHistory, setUserPurchaseHistory] = useState(null);

    // SignalR
    const [connection, setConnection] = useState(null);

    const handleEnterSystem = async () => {
        EnterSystem().then(response => response.ok ? 
            response.json().then(id => setUser({ ...user, id }) ) : printErrorMessage(response)).catch(err => alert(err));
    }

    // TODO: Catch error messages correctly from API

    //#region Cart Functionality 
    
    const fetchCart = async () => {
        if (user.id !== -1){
            GetUserShoppingCart(user.id).then(response => response.ok ? 
                response.json().then(data => setCart({id: data.id,
                                                    products: data.shoppingBags.length === 0 ? [] : 
                                                                data.shoppingBags.reduce(function(list, bag) {
                                                                    return list.concat(bag.products.map(p => ({ ...p, storeID: bag.storeId })));
                                                                }, []),
                                                    totalPrice: data.shoppingBags.length !== 0 ? handleGetTotalShoppingCartPrice() : 0
                                                                // data.shoppingBags.reduce(function(total, bag) {
                                                                //                 return total + bag.totalBagPrice;
                                                                //             }, 0)
                                                    })) : null)
                                .catch(err => console.log(err));
        }
    }

    const handleAddToCart = async (storeId, productId, name, price, quantity, image) => {
        AddProductToCart({ userID: user.id, productID: productId, ProductQuantity: quantity, storeID: storeId }).then(response => response.ok ? 
            response.json().then(status => status && (alert("Product added successfully") & fetchCart())) : printErrorMessage(response)).catch(err => alert(err));

    }

    const handleUpdateCartQuantity = async (storeID, productID, quantity) => {
        const data = { userID: user.id, storeID, productID, quantity };

        UpdateShoppingCart(data).then(response => response.ok ? 
            response.json().then(result => result.execStatus ? fetchCart() : console.log(result.message)) : console.log("NOT OKAY")).catch(err => alert(err));
    }

    // const handleUpdateCartQuantity = async (productId, quantity) => {
        
    //     if (quantity === 0) {
    //         handleRemoveFromCart(productId);
    //     } else {
    //         setCart(function(prevState) {
    //             const product = prevState.products.filter(product => product.id === productId)[0];
    //             const toAdd = quantity - product.quantity;
                
    //             return {
    //                 products: prevState.products.map(
    //                 p => p.id === productId ? { ...p, quantity: quantity }: p
    //                 ),
    //                 totalPrice: prevState.totalPrice + product.price * toAdd       
    //             }
    //         }); 
    //     }
    // }

    // const handleRemoveFromCart = async (productId) => {
    //     setCart(function(prevState) {
    //         const product = prevState.products.filter(product => product.id === productId)[0];
            
    //         return {
    //         products: prevState.products.filter(
    //             product => product.id !== productId
    //         ),
    //         totalPrice: prevState.totalPrice - product.price * product.quantity       
    //         }
    //     }); 
    // }

    const handleEmptyCart = () => {
        console.log("EMPTY CART");
        const allProducts = cart.products;
        allProducts.map((product) => handleUpdateCartQuantity(product.storeID, product.id, 0));

        // setCart({products: [], totalPrice: 0});
    }

    const fakeEmptyCart = () => {
        setCart({products: [], totalPrice: 0});
    };

    const handleGetTotalShoppingCartPrice = () => {
        GetTotalShoppingCartPrice(user.id).then(response => response.ok ? 
            response.json().then(result => result.execStatus ? setCart(function(prevState) {
                return { ...prevState, totalPrice: result.data };

            }) : console.log(result.message)) : console.log("NOT OKAY")).catch(err => alert(err));
    }
    //#endregion

    //#region User Functionality

    const handleLogin = async (data) => {
        LoginAPI({ ...data, guestuserid: user.id }).then(response => response.ok ? 
            response.json().then(id => 
                reIdentify(user.id, id, 'Login')
                .then(() => setUser({ id, email: data.email, loggedIn: true}))
                .catch(err => alert(err))) : printErrorMessage(response)).catch(err => alert(err));
    }

    const handleRegister = async (data) => {
        RegisterAPI(data).then(response => response.ok ? 
            response.json().then(id => console.log(id)) : printErrorMessage(response)).catch(err => alert(err));
    }

    const handleLogOut = () => {
        Logout(user.email).then(response => response.ok ?
            response.json().then(id => 
                reIdentify(user.id, id, 'Logout')
                .then(() => setUser({id, email: '', loggedIn: false}) & fakeEmptyCart())
                .catch(err => alert(err))) : printErrorMessage(response)).catch(err => alert(err));
    }
    
    const handleOpenNewStore = async (data) => {
        OpenNewStore({ userID: user.id, ...data }).then(response => response.ok ?
             response.json().then(message => alert(message)) : printErrorMessage(response)).catch(err => alert(err));
    }

    const handleGetUserPurchaseHistory = () => {
        GetUserPurchaseHistory(user.id).then(response => response.ok ? 
            response.json().then(result => result.execStatus ? setUserPurchaseHistory(result.data) : console.log(result.message)) : console.log("NOT OKAY")).catch(err => alert(err));
    }
    //#endregion

    //#region System Admin Functionality

    const handleAddSystemAdmin = async (data) => {
        AddSystemAdmin(user.id, data.email).then(response => response.ok ? 
            response.json().then(id => setSystemAdmins(prev => [...prev, id]) & alert("System Admin added Successfully")) : printErrorMessage(response)).catch(err => alert(err));
    }

    const handleRemoveSystemAdmin = async (data) => {
        RemoveSystemAdmin(user.id, data.email).then(response => response.ok ? 
            response.json().then(result => setSystemAdmins(prev => prev.filter(id => id !== result.data.id)) & alert(result.message)) : printErrorMessage(response)).catch(err => alert(err));
    }

    // TODO: Check
    const handleResetSystem = async () => {
        ResetSystem(user.id).then(response => response.ok ? 
            response.json().then(message => alert(message)) : printErrorMessage(response)).catch(err => alert(err));
    }

    //#endregion

    const handleStoreSearch = async (query) => {
        setStoreSearchQuery(query);
    }

    const handleProductSearch = async (query) => {
        setProductSearchQuery(query);
    }

    // Serach by keywords, if not found by category, if not found by name.
    const searchProductsByQuery = async () => {
        const query = {Name: productSearchQuery};
        console.log(query);
        SearchProduct({ Keywords: [productSearchQuery] })
            .then(response => response.json()
            .then(result => result.execStatus ? setProducts(result.data) 
                                : SearchProduct({ Category: productSearchQuery })
                                    .then(response => response.json()
                                    .then(result => result.execStatus ? setProducts(result.data) : 
                                                        SearchProduct({ Name: productSearchQuery })
                                                        .then(response => response.json()
                                                        .then(result => result.execStatus ? setProducts(result.data) : console.log(result.message)))
                                                        .catch(err => console.log(err))))
                                    .catch(err => console.log(err))))
            .catch(err => console.log(err));

        // SearchProduct({ Category: productSearchQuery }).then(response => response.json().then(result => result.execStatus ? setProducts(result.data) : alert(result.message))).catch(err => console.log(err));
    }

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
        if (productSearchQuery !== '')
            searchProductsByQuery();
        else
            setProducts([]);
    }, [productSearchQuery]);

    // useEffect(() => {
    //     console.log(cart.products);
    // }, [cart]);

    useEffect(() => {
        console.log(systemAdmins);
    }, [systemAdmins]);

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
                    {/* Navigation Bar */}
                    <Navbar storeId={-1} totalItems={cart.products.length} user={user} isSystemAdmin={systemAdmins.includes(user.id)} handleLogOut={handleLogOut} 
                            handleSearch={handleProductSearch} handleGetUserPurchaseHistory={handleGetUserPurchaseHistory} />
                    
                    <Switch>
                        {/* Cart Page */}
                        <Route exact path="/cart">
                            <Cart
                                id={0} 
                                cart={cart} 
                                handleUpdateCartQuantity={handleUpdateCartQuantity} 
                                handleEmptyCart={handleEmptyCart} 
                            />
                        </Route>

                        {/* Register Page */}
                        <Route path="/register" render={(props) => (<Register handleRegister={handleRegister} {...props} />)} />
                        
                        {/* Login Page */}
                        <Route path="/login" render={(props) => (<Login handleLogin={handleLogin} {...props} />)} />

                        {/* Checkout Page */}
                        <Route exact path="/checkout">
                            <Checkout userID={user.id} cart={cart} handleEmptyCart={handleEmptyCart} />
                        </Route>
                        
                        {/* Open New Store Page */}
                        <Route exact path={`/${user.id}/openstore`} 
                                render={(props) => (<Action name='Open New Store' fields={[{name: 'Store Name', required: true}]} 
                                                            handleAction={handleOpenNewStore} {...props} />)} 
                        />

                        {/* Purchase History Page */}
                        <Route exact path={`/${user.id}/purchasehistory`}
                                render = {() => (<Review checkoutToken={userPurchaseHistory} />)} />
                        
                        {/* Add System Admin Page */}
                        {systemAdmins.includes(user.id) && (
                            <Route exact path={`/${user.id}/addsystemadmin`} 
                                    render={(props) => (<Action name='Add System Admin' fields={[{name: 'Email', required: true}]} 
                                                                handleAction={handleAddSystemAdmin} {...props} />)} 
                            />
                        )}

                        {/* Remove System Admin Page */}
                        {systemAdmins.includes(user.id) && (
                            <Route exact path={`/${user.id}/removesystemadmin`} 
                                    render={(props) => (<Action name='Remove System Admin' fields={[{name: 'Email', required: true}]} 
                                                                handleAction={handleRemoveSystemAdmin} {...props} />)} 
                            />
                        )}

                        {/* Reset System Page */}
                        {systemAdmins.includes(user.id) && (
                            <Route exact path={`/${user.id}/resetsystem`} 
                                    render={(props) => (<Action name='Reset System' 
                                                                handleAction={handleResetSystem} {...props} />)} 
                            />
                        )}
                        
                        {/* Main Page - Stores / Search results */}
                        { products.length > 0 ? (
                            <Products storeName={'SEARCH_RES'} products={products} onAddToBag={handleAddToCart} />
                        ) : (
                            <Route path="/" render={(props) => (<Stores user={user} searchQuery={storeSearchQuery} handleAddToCart={handleAddToCart} handleLogOut={handleLogOut} {...props} />)} />
                        )}
                        
                    </Switch>
                </div>
            </Router>
        </MuiThemeProvider>
    )
}

export default App;
