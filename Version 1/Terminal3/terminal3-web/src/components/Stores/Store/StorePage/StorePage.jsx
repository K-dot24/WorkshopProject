import React, { useState, useEffect } from 'react'
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';   // don't remove Router

import { Products, Navbar, Cart, Action } from '../../../../components';
import { GetAllProductByStoreIDToDisplay, AddProductToStore, RemoveProductFromStore } from '../../../../api/API';



const StorePage = ({ store, user, match, handleAddToCart, handleLogOut }) => {
    const [products, setProducts] = useState([]);
    const [bag, setBag] = useState({products: [], totalPrice: 0});

    const fetchProducts = async () => {
        GetAllProductByStoreIDToDisplay(store.id).then(response => response.json().then(json => setProducts(json))).catch(err => console.log(err));
    }

    // TODO: Fetch real data from API
    const fetchBag = async () => { 
    }

     // TODO: Update with real data
     const handleAddToBag = async (productId, name, price, quantity, image) => {
         setBag(function(prevState) {
            const productArr = prevState.products.filter(p => p.id === productId);
            
            return {
                products: productArr.length === 0 ? [...prevState.products, {id: productId, name, price, quantity, image}] : handleUpdateBagQuantity(productId, productArr[0].quantity + quantity),
                totalPrice: prevState.totalPrice + price*quantity      
            }
        }); 
    }

    const handleUpdateBagQuantity = async (productId, quantity) => {
        if (quantity === 0) {
            handleRemoveFromBag(productId);
        } else {
            setBag(function(prevState) {
                const product = prevState.products.filter(product => product.id === productId)[0];
                const toAdd = quantity - product.quantity;
                
                return {
                    products: prevState.products.map(
                    p => p.id === productId ? { ...p, quantity: quantity }: p
                    ),
                    totalPrice: prevState.totalPrice + product.price * toAdd       
                }
            });
        }
    }

    const handleRemoveFromBag = async (productId) => {

        setBag(function(prevState) {
            const product = prevState.products.filter(product => product.id === productId)[0];
            
            return {
            products: prevState.products.filter(
                product => product.id !== productId
            ),
            totalPrice: prevState.totalPrice - product.price * product.quantity       
            }
        }); 
    }

    const handleEmptyBag = async () => {
        setBag({products: [], totalPrice: 0});
    }

    //#region Staff Actions

    const handleAddProductToStore = async (data) => {
        let keywords = [];
        if (data.keywords) {
            keywords = data.keywords.split(',');
        }

        // To take only product ID if needed: message.substring(message.indexOf(":") + 2)
        AddProductToStore({ userID: user.id, storeID: store.id, ...data, keywords }).then(response => response.ok ?
            response.json().then(message => console.log(message)) : console.log("NOT OK")).catch(err => console.log(err));
        
    }

    const handleRemoveProductFromStore = async (data) => {
        console.log({userID: user.id, storeID: store.id, ...data});

        RemoveProductFromStore(user.id, store.id, data.productid).then(response => response.ok ?
            response.json().then(message => console.log(message)) : console.log("NOT OK")).catch(err => console.log(err));
    }

    //#endregion

    useEffect(() => {
        fetchProducts();
        fetchBag();
    }, []);

    return (
        <div>
            <Navbar storeId={store.id} totalItems={bag.products.length} user={user} handleLogOut={handleLogOut} />
            <Switch>
                <Route exact path={match.url}>
                    <Products storeName={store.name} products={products} onAddToBag={handleAddToBag} />
                </Route>

                <Route exact path={match.url + "/bag"}>
                    <Cart
                        id={store.id}
                        cart={bag}
                        handleUpdateCartQuantity={handleUpdateBagQuantity} 
                        handleRemoveFromCart={handleRemoveFromBag} 
                        handleEmptyCart={handleEmptyBag}
                        handleAddToCart={handleAddToCart}
                    />
                </Route>

                <Route exact path={match.url + `/addnewproduct`} 
                    render={(props) => (<Action name='Add New Product' 
                                                fields={[{name: 'Product Name', required: true}, 
                                                        {name: 'Price', required: true, type: "number"}, 
                                                        {name: 'Initial Quantity', required: true, type: "number"},
                                                        {name: 'Category', required: true},
                                                        {name: 'Keywords', required: false}]} 
                                                handleAction={handleAddProductToStore} {...props} />)} 
                />

                <Route exact path={match.url + `/removeproduct`} 
                    render={(props) => (<Action name='Remove Product' 
                                                fields={[{name: 'Product ID', required: true}]} 
                                                handleAction={handleRemoveProductFromStore} {...props} />)} />

            </Switch>
        </div>
    )
}

export default StorePage;
