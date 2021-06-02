import React, { useState, useEffect } from 'react'
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';   // don't remove Router

import { Products, Navbar, Cart, Action, CheckboxList } from '../../../../components';
import { GetAllProductByStoreIDToDisplay, AddProductToStore, RemoveProductFromStore, EditProductDetails, 
        AddStoreOwner, AddStoreManager, RemoveStoreManager, GetStoreStaff, SetPermissions, SearchProduct,
        printErrorMessage, RemovePermissions, GetPermission, AddDiscountPolicy, AddDiscountPolicyById,
        AddDiscountCondition, RemoveDiscountPolicy, RemoveDiscountCondition, GetDiscountPolicyData,
        AddPurchasePolicy, AddPurchasePolicyById, RemovePurchasePolicy, GetPurchasePolicyData } from '../../../../api/API';



const StorePage = ({ store, user, match, handleAddToCart, handleLogOut }) => {
    const [products, setProducts] = useState([]);
    const [bag, setBag] = useState({products: [], totalPrice: 0});

    const fetchProducts = async () => {
        GetAllProductByStoreIDToDisplay(store.id).then(response => response.json().then(json => setProducts(json))).catch(err => console.log(err));
    }

    //#region Bag Functionality

    const handleAddToBag = async (productId, name, price, quantity, image) => {
         setBag(function(prevState) {
            const productArr = prevState.products.filter(p => p.id === productId);
            
            return {
                products: productArr.length === 0 ? [...prevState.products, {id: productId, name, price, quantity, image}] : handleUpdateBagQuantity(0, productId, productArr[0].quantity + quantity),
                totalPrice: prevState.totalPrice + price*quantity      
            }
        }); 
    }

    const handleUpdateBagQuantity = async (storeID, productId, quantity) => {
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
        AddProductToStore({ userID: user.id, storeID: store.id, ...data, keywords })
        .then(response => response.json().then(message => alert(message))).catch(err => console.log(err));
        
    }

    const handleRemoveProductFromStore = async (data) => {
        RemoveProductFromStore(user.id, store.id, data.productid)
        .then(response => response.json().then(message => alert(message))).catch(err => console.log(err));
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
            response.json().then(message => alert(message)) : printErrorMessage(response)).catch(err => console.log(err));
    }

    const handleAddStoreOwner = async (data) => {  
        AddStoreOwner({ addedOwnerID: data.newownerid, currentlyOwnerID: user.id, storeID: store.id }).then(response => response.ok ?
            response.json().then(message => alert(message)) : printErrorMessage(response)).catch(err => console.log(err));
    }

    const handleAddStoreManager = async (data) => {
        console.log(data);
        
        AddStoreManager({ addedManagerID: data.newmanagerid, currentlyOwnerID: user.id, storeID: store.id }).then(response => response.ok ?
            response.json().then(message => alert(message)) : printErrorMessage(response)).catch(err => console.log(err));
    }

    const handleRemoveStoreManager = async (data) => {
        RemoveStoreManager(store.id, user.id, data.removedmanagerid).then(response => response.ok ?
            response.json().then(message => alert(message)) : printErrorMessage(response)).catch(err => console.log(err));
    }

    // TODO: Show information to user
    const handleGetStoreStaff = async () => {
        GetStoreStaff(user.id, store.id).then(response => response.ok ?
            response.json().then(json => console.log(json)) : printErrorMessage(response)).catch(err => console.log(err));
    }

    const handleSetPermissions = (permissions, managerID) => {
        console.log(permissions);

        GetPermission(managerID.managerid, store.id).then(response => response.ok ? 
            response.json().then(function(prevPermissions) {
                const boolToNums = prevPermissions.reduce(
                    (out, bool, index) => bool ? out.concat(index) : out, 
                    []
                  );
                handleRemovePermissions(boolToNums, managerID);

                return SetPermissions({storeID: store.id, managerID: managerID.managerid, ownerID: user.id, permissions})
                        .then(response => response.ok ?
                            response.json().then(message => alert(message)) : printErrorMessage(response)).catch(err => console.log(err));     

            }) : null).catch(err => console.log(err));

        // SetPermissions({storeID: store.id, managerID: managerID.managerid, ownerID: user.id, permissions})
        //     .then(response => response.ok ?
        //         response.json().then(message => alert(message)) : printErrorMessage(response)).catch(err => console.log(err));
    }

    const handleRemovePermissions = (permissions, managerID) => {
        RemovePermissions({storeID: store.id, managerID: managerID.managerid, ownerID: user.id, permissions})
            .then(response => response.ok ?
                response.json().then(message => console.log(message)) : printErrorMessage(response)).catch(err => console.log(err));
    }

    //#region Discount Policy Functions

    const handleAddDiscountPolicy = (data, mainType, subType) => {
        let info;
        let allData;
        
        switch (mainType) {
            case "VisibleDiscount":
                info = { type: mainType, ExpirationDate: data.expirationdate, Percentage: data.percentage, Target: { type: subType }  };

                if ('categories' in data) {
                    const categories_array = data.categories.split(',');
                    info = { ...info, Target: { type: subType, Categories: categories_array }};
                } 
                if ('productsid' in data) {
                    const productsid_array = data.productsid.split(',');
                    info = { ...info, Target: { type: subType, ProductIds: productsid_array } };
                }
                break;

            case "DiscreetDiscount":
                info = { type: mainType, DiscountCode: data.discountcode };
                break;

            default:
                info = { type: mainType };
        }

        allData = { storeId: store.id, info };
        // console.log(allData);

        if ('nodeid' in data) {
            AddDiscountPolicyById(data.nodeid, allData).then(response => response.ok ? 
                response.json().then(result => alert(result.message)) : printErrorMessage(response)).catch(err => alert(err));
        } else {
            AddDiscountPolicy(allData).then(response => response.ok ? 
                response.json().then(result => alert(result.message)) : printErrorMessage(response)).catch(err => alert(err));
        }
    }

    const handleAddDiscountCondition = (data, type, sub) => {
        let info;

        switch (type) {
            case "MaxProductCondition":
                info = { type, MaxQuantity: data.maxquantity, ProductId: data.productid  };
                break;

            case "MinProductCondition":
                info = { type, MinQuantity: data.minquantity, ProductId: data.productid };
                break;

            case "MinBagPriceCondition":
                info = { type, MinPrice: data.minprice };
                break;

            default:
                info = { type };
        }

        const allData = { storeId: store.id, info };
        // console.log(allData);

        AddDiscountCondition(data.nodeid, allData).then(response => response.ok ? 
            response.json().then(result => alert(result.message)) : printErrorMessage(response)).catch(err => alert(err));

    }

    const handleRemoveDiscountPolicy = (data) => {
        RemoveDiscountPolicy(store.id, data.nodeid).then(response => response.ok ? 
            response.json().then(result => alert(result.message)) : printErrorMessage(response)).catch(err => alert(err));
    }

    const handleRemoveDiscountCondition = (data) => {
        RemoveDiscountCondition(store.id, data.nodeid).then(response => response.ok ? 
            response.json().then(result => alert(result.message)) : printErrorMessage(response)).catch(err => alert(err));
    }

    // TODO: Display information to user (+ check business returns actual data)
    const handleGetDiscountPolicyData = () => {
        GetDiscountPolicyData(store.id).then(response => response.ok ? 
            response.json().then(result => console.log(result)) : printErrorMessage(response)).catch(err => alert(err));
    }

    //#endregion

    //#region Purchase Policy Functions

    const handleAddPurchasePolicy = (data, mainType, subType) => {
        let info;
        let allData;
        
        switch (mainType) {
            case "MaxProductPolicy":
                info = { type: mainType, ProductId: data.productid, Max: data.maximumquantity };
                break;

            case "MinProductPolicy":
                info = { type: mainType, ProductId: data.productid, Min: data.minimumquantity };
                break;

            case "MinAgePolicy":
                info = { type: mainType, Age: data.minimumage };
                break;
        
            case "RestrictedHoursPolicy":
                info = { type: mainType, StartRestrict: data.restrictionstart, EndRestrict: data.restrictionend, ProductId: data.productid};
                break;

            default:
                info = { type: mainType };
        }

        allData = { storeId: store.id, info };
        // console.log(allData);

        if ('nodeid' in data) {
            AddPurchasePolicyById(data.nodeid, allData).then(response => response.ok ? 
                response.json().then(result => alert(result.message)) : printErrorMessage(response)).catch(err => alert(err));
        } else {
            AddPurchasePolicy(allData).then(response => response.ok ? 
                response.json().then(result => alert(result.message)) : printErrorMessage(response)).catch(err => alert(err));
        }
    }

    const handleRemovePurchasePolicy = (data) => {
        RemovePurchasePolicy(store.id, data.nodeid).then(response => response.ok ? 
            response.json().then(result => alert(result.message)) : printErrorMessage(response)).catch(err => alert(err));
    }

    // TODO: Display information to user (+ check business returns actual data)
    const handleGetPurchasePolicyData = () => {
        GetPurchasePolicyData(store.id).then(response => response.ok ? 
            response.json().then(result => console.log(result)) : printErrorMessage(response)).catch(err => alert(err));
    }

    //#endregion

    //#endregion

    //#region Search (commented out)

    // const [searchQuery, setsearchQuery] = useState('');
    
    // const searchProductsByQuery = async () => {
    //     const query = {Name: searchQuery};
    //     console.log(query);
    //     SearchProduct(query).then(response => response.json().then(json => console.log(json))).catch(err => console.log(err));
    // }

    // const handleProductSearch = async (query) => {
    //     setsearchQuery(query);
    // }

    // useEffect(() => {
    //     
    //     if (searchQuery !== '')
    //         searchProductsByQuery();
    //     // else
    //     //     fetchStores();
    // }, [searchQuery]);

    //#endregion

    useEffect(() => {
        fetchProducts();
        console.log("store id: " + store.id);
        console.log(products);
    }, []);

    return (
        <div>
            <Navbar storeId={store.id} totalItems={bag.products.length} user={user} handleLogOut={handleLogOut} 
                    /*handleSearch={handleProductSearch}*/ />
            <Switch>
                <Route exact path={match.url}>
                    <Products storeName={store.name} products={products} onAddToBag={handleAddToBag} />
                </Route>

                <Route exact path={match.url + "/bag"}>
                    <Cart
                        id={store.id}
                        cart={bag}
                        handleUpdateCartQuantity={handleUpdateBagQuantity} 
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

                {/* Add Discount Policy */}
                <Route exact path={match.url + `/discountpolicy/adddiscountpolicy`} 
                    render={(props) => (<Action name='Add Discount Policy'
                                                fields={[{name: 'Expiration Date', required: true, type: 'date', belongsTo: 'VisibleDiscount'},
                                                        {name: 'Percentage', required: true, type: 'number', belongsTo: 'VisibleDiscount'},
                                                        {name: 'Node ID', required: false},
                                                        {name: 'Categories', required: true, belongsTo: 'DiscountTargetCategories'},
                                                        {name: 'Products ID', required: true, belongsTo: 'DiscountTargetProducts'},
                                                        {name: 'Discount Code', required: true, belongsTo: 'DiscreetDiscount'}]}   
                                                mainTypes={[{name: 'VisibleDiscount'}, {name: 'DiscreetDiscount'},
                                                        {name: 'ConditionalDiscount'}, {name: 'DiscountAddition'},
                                                        {name: 'DiscountAnd'}, {name: 'DiscountMax'},
                                                        {name: 'DiscountMin'}, {name: 'DiscountOr'},
                                                        {name: 'DiscountXor'}]}
                                                subTypes={[
                                                            {main: 'VisibleDiscount', 
                                                            subs: [{ name: 'DiscountTargetShop'},
                                                                    {name: 'DiscountTargetCategories'},
                                                                    {name: 'DiscountTargetProducts'}]}
                                                        ]}
                                                handleAction={handleAddDiscountPolicy} {...props} />)} 
                />

                {/* Add Discount Condition */}
                <Route exact path={match.url + `/discountpolicy/adddiscountcondition`} 
                    render={(props) => (<Action name='Add Discount Condition'
                                                fields={[{name: 'Node ID', required: true},
                                                        {name: 'Product ID', required: true, belongsTo: 'MaxProductCondition'},
                                                        {name: 'Max Quantity', required: true, type: 'number', belongsTo: 'MaxProductCondition'},
                                                        {name: 'Product ID', required: true, belongsTo: 'MinProductCondition'},
                                                        {name: 'Min Quantity', required: true, type: 'number', belongsTo: 'MinProductCondition'},
                                                        {name: 'Min Price', required: true, type: 'number', belongsTo: 'MinBagPriceCondition'}]}   
                                                mainTypes={[{name: 'DiscountConditionAnd'}, {name: 'DiscountConditionOr'},
                                                        {name: 'MaxProductCondition'}, {name: 'MinProductCondition'},
                                                        {name: 'MinBagPriceCondition'}]}
                                                handleAction={handleAddDiscountCondition} {...props} />)} 
                />

                { /* Remove Discount Policy */}
                <Route exact path={match.url + `/discountpolicy/removediscountpolicy`} 
                    render={(props) => (<Action name='Remove Discount Policy'
                                                fields={[{name: 'Node ID', required: true}]}   
                                                handleAction={handleRemoveDiscountPolicy} {...props} />)} 
                />

                { /* Remove Discount Condition */}
                <Route exact path={match.url + `/discountpolicy/removediscountcondition`} 
                    render={(props) => (<Action name='Remove Discount Condition'
                                                fields={[{name: 'Node ID', required: true}]}   
                                                handleAction={handleRemoveDiscountCondition} {...props} />)} 
                />

                { /* Get Discount Policy Data */}
                <Route exact path={match.url + `/getdiscountpolicy`} 
                    render={(props) => (<Action name='Get Discount Policy Data'  
                                                handleAction={handleGetDiscountPolicyData} {...props} />)} 
                />

                {/* Add Purchase Policy */}
                <Route exact path={match.url + `/purchasepolicy/addpurchasepolicy`} 
                    render={(props) => (<Action name='Add Purchase Policy'
                                                fields={[{name: 'Node ID', required: false},
                                                        {name: 'Product ID', required: true, belongsTo: 'MaxProductPolicy'},
                                                        {name: 'Maximum Quantity', required: true, type: 'number', belongsTo: 'MaxProductPolicy'},
                                                        {name: 'Product ID', required: true, belongsTo: 'MinProductPolicy'},
                                                        {name: 'Minimum Quantity', required: true, type: 'number', belongsTo: 'MinProductPolicy'},
                                                        {name: 'Minimum Age', required: true, type: 'number', belongsTo: 'MinAgePolicy'},
                                                        {name: 'Product ID', required: true, belongsTo: 'RestrictedHoursPolicy'},
                                                        {name: 'Restriction Start', required: true, type: 'date', belongsTo: 'RestrictedHoursPolicy'},
                                                        {name: 'Restriction End', required: true, type: 'date', belongsTo: 'RestrictedHoursPolicy'}]}   
                                                mainTypes={[{name: 'AndPolicy'}, {name: 'OrPolicy'},
                                                        {name: 'ConditionalPolicy'}, {name: 'MaxProductPolicy'},
                                                        {name: 'MinProductPolicy'}, {name: 'MinAgePolicy'},
                                                        {name: 'RestrictedHoursPolicy'}]}
                                                handleAction={handleAddPurchasePolicy} {...props} />)} 
                />

                { /* Remove Purchase Policy */}
                <Route exact path={match.url + `/purchasepolicy/removepurchasepolicy`} 
                    render={(props) => (<Action name='Remove Purchase Policy'
                                                fields={[{name: 'Node ID', required: true}]}   
                                                handleAction={handleRemovePurchasePolicy} {...props} />)} 
                />

                { /* Get Purchase Policy Data */}
                <Route exact path={match.url + `/getpurchasepolicy`} 
                    render={(props) => (<Action name='Get Purchase Policy Data'  
                                                handleAction={handleGetPurchasePolicyData} {...props} />)} 
                />


            </Switch>
        </div>
    )
}

export default StorePage;
