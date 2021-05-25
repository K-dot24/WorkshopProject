import React, { useState, useEffect } from 'react'
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';   // don't remove Router

import { Products, Navbar, Cart, Action, CheckboxList } from '../../../../components';
import { GetAllProductByStoreIDToDisplay, AddProductToStore, RemoveProductFromStore, EditProductDetails, 
        AddStoreOwner, AddStoreManager, RemoveStoreManager, GetStoreStaff, SetPermissions, SearchProduct } from '../../../../api/API';



const StorePage = ({ store, user, match, handleAddToCart, handleLogOut }) => {
    const [products, setProducts] = useState([]);
    const [bag, setBag] = useState({products: [], totalPrice: 0});
    const [searchQuery, setsearchQuery] = useState('');

    const fetchProducts = async () => {
        GetAllProductByStoreIDToDisplay(store.id).then(response => response.json().then(json => setProducts(json))).catch(err => console.log(err));
    }

    //#region Bag Functionality

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

    //#endregion

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
        RemoveProductFromStore(user.id, store.id, data.productid).then(response => response.ok ?
            response.json().then(message => console.log(message)) : console.log("NOT OK")).catch(err => console.log(err));
    }

    const handleEditProductDetails = async (data) => {
        for (var key in data) {
            if (data[key] === null || data[key] === "" || (typeof data[key] !== 'string' && !(data[key] instanceof String) && isNaN(data[key]))){
                delete data[key]
            }
        }

        const productID = data.productid;
        delete data.productid;

        const details = { ...data };
        data = { productID, details};

        console.log({ userID: user.id, storeID: store.id, ...data });

        EditProductDetails({ userID: user.id, storeID: store.id, ...data }).then(response => response.ok ?
            response.json().then(message => console.log(message)) : console.log("NOT OK")).catch(err => console.log(err));
    }

    const handleAddStoreOwner = async (data) => {  
        AddStoreOwner({ addedOwnerID: data.newownerid, currentlyOwnerID: user.id, storeID: store.id }).then(response => response.ok ?
            response.json().then(message => console.log(message)) : console.log("NOT OK")).catch(err => console.log(err));
    }

    const handleAddStoreManager = async (data) => {
        console.log(data);
        
        AddStoreManager({ addedManagerID: data.newmanagerid, currentlyOwnerID: user.id, storeID: store.id }).then(response => response.ok ?
            response.json().then(message => console.log(message)) : console.log("NOT OK")).catch(err => console.log(err));
    }

    const handleRemoveStoreManager = async (data) => {
        console.log(data);

        RemoveStoreManager(store.id, user.id, data.removedmanagerid).then(response => response.ok ?
            response.json().then(json => console.log(json)) : console.log("NOT OK")).catch(err => console.log(err));
    }

    const handleGetStoreStaff = async () => {
        GetStoreStaff(user.id, store.id).then(response => response.ok ?
            response.json().then(json => console.log(json)) : console.log("NOT OK")).catch(err => console.log(err));
    }

    const handleSetPermissions = async (permissions, managerID) => {
        console.log(permissions, managerID);

        SetPermissions({storeID: store.id, managerID: managerID.managerid, ownerID: user.id, permissions}).then(response => response.ok ?
            response.json().then(json => console.log(json)) : console.log("NOT OK")).catch(err => console.log(err));
    }

    //#endregion

    // TODO: SearchStore- fix API and check
    const searchProductsByQuery = async () => {
        const query = {Name: searchQuery};
        console.log(query);
        SearchProduct(query).then(response => response.json().then(json => console.log(json))).catch(err => console.log(err));
    }

    const handleProductSearch = async (query) => {
        setsearchQuery(query);
    }

    useEffect(() => {
        // TODO: Check after API works
        if (searchQuery !== '')
            searchProductsByQuery();
        // else
        //     fetchStores();
    }, [searchQuery]);

    useEffect(() => {
        fetchProducts();
        console.log("store id: " + store.id);
    }, []);

    return (
        <div>
            <Navbar storeId={store.id} totalItems={bag.products.length} user={user} handleLogOut={handleLogOut} handleSearch={handleProductSearch} />
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
                                                handleAction={handleRemoveProductFromStore} {...props} />)} 
                />

                <Route exact path={match.url + `/editproductdetails`} 
                    render={(props) => (<Action name='Edit Product Details' 
                                                fields={[{name: 'Product ID', required: true}, 
                                                        {name: 'Name', required: false}, 
                                                        {name: 'Price', required: false, type: "number"},
                                                        {name: 'Quantity', required: false, type: "number"},
                                                        {name: 'Category', required: false},
                                                        {name: 'Keywords', required: false}]} 
                                                handleAction={handleEditProductDetails} {...props} />)} 
                />

                <Route exact path={match.url + `/addstoreowner`} 
                    render={(props) => (<Action name='Add Store Owner' 
                                                fields={[{name: 'New Owner ID', required: true}]} 
                                                handleAction={handleAddStoreOwner} {...props} />)} 
                />

                <Route exact path={match.url + `/addstoreManager`} 
                    render={(props) => (<Action name='Add Store Manager' 
                                                fields={[{name: 'New Manager ID', required: true}]} 
                                                handleAction={handleAddStoreManager} {...props} />)} 
                />

                <Route exact path={match.url + `/removestoremanager`} 
                    render={(props) => (<Action name='Remove Store Manager' 
                                                fields={[{name: 'Removed Manager ID', required: true}]} 
                                                handleAction={handleRemoveStoreManager} {...props} />)} 
                />

                <Route exact path={match.url + `/getstorestaff`} 
                    render={(props) => (<Action name='Get Store Stuff'     
                                                handleAction={handleGetStoreStaff} {...props} />)} 
                />

                <Route exact path={match.url + `/setpermissions`} 
                    render={(props) => (<Action name='Set Permissions'
                                                fields={[{name: 'Manager ID', required: true}]}   
                                                handleAction={handleSetPermissions} {...props} />)} 
                />

            </Switch>
        </div>
    )
}

export default StorePage;
