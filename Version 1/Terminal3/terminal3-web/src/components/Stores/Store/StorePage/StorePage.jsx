import React, { useState, useEffect } from 'react'
import { useLocation, useParams, BrowserRouter as Router, Switch, Route } from 'react-router-dom';   // don't remove Router

import { Products, Navbar, Cart } from '../../../index';
import { GetAllProductByStoreIDToDisplay } from '../../../../api/API';



const StorePage = ({ handleAddToCart, match, user, handleLogOut }) => {
    const { id } = useParams();
    const { state } = useLocation();    // passed from <Store> Link

    let store = null;
    if (state) {
        store = state.store;
        console.log(store);
    }
    
    const [products, setProducts] = useState([]);
    const [bag, setBag] = useState({products: [], totalPrice: 0});

    // TODO: Fetch real data from API
    const fetchProducts = async () => {
        GetAllProductByStoreIDToDisplay(id).then(response => response.json().then(json => setProducts(json))).catch(err => console.log(err));

        
        // Mock Data
        // setProducts([
        //     { id: 1, name: 'Nike Blazer Mid 77', price: 450.0, quantity: 10, category: 'Shoes', rating: 5, numberOfRates: 2, keywords: ['nike', 'shoes', 'style'], image: 'https://static.nike.com/a/images/t_PDP_1280_v1/f_auto,q_auto:eco/ef4dbed6-c621-4879-8db3-f87296bfb570/blazer-mid-77-vintage-shoe-CBDjT0.png'},
        //     { id: 2, name: 'Macbook', price: 4019.99, quantity: 50, category: 'Laptops', rating: 4.5, numberOfRates: 6, keywords: ['apple', 'macbook', 'expensive'], image: 'https://d3m9l0v76dty0.cloudfront.net/system/photos/5435150/large/d6b55817aafd21bc9c896dfcfcaf0ae7.jpg'}
        // ]);
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

    useEffect(() => {
        fetchProducts();
        fetchBag();
    }, []);

    return (
        <div>
            <Navbar storeId={id} totalItems={bag.products.length} user={user} handleLogOut={handleLogOut} />
            <Switch>
                <Route exact path={match.url}>
                    <Products storeName={store !== null ? store.name : ''} products={products} onAddToBag={handleAddToBag} />
                </Route>

                <Route exact path={match.url + "/bag"}>
                    <Cart
                        id={id}
                        cart={bag}
                        handleUpdateCartQuantity={handleUpdateBagQuantity} 
                        handleRemoveFromCart={handleRemoveFromBag} 
                        handleEmptyCart={handleEmptyBag}
                        handleAddToCart={handleAddToCart}
                    />
                </Route>
            </Switch>
        </div>
    )
}

export default StorePage;
