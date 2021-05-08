//#region to delete
// import fetch from 'node-fetch';
// import https from 'https';

// const httpsAgent = new https.Agent({
//     rejectUnauthorized: false,
//   });


        // option 1
        // Axios  
        //     .get("http://localhost:5001/api/Data/GetAllStoresToDisplay")  
        //     .then(result => setData(result.data))
        //     .catch((error) => {
        //         console.log(error);
        //     });
        // console.log(data); 

        // option 2
        // const fetchData = async () => {
        //     const result = await axios(
        //       'http://localhost:5000/api/Data/GetAllStoresToDisplay',
        //     );
       
        //     setData(result.data);
        // };
        // fetchData();
        // console.log(data);
//#endregion

export const products_image_url = 'https://i.ibb.co/HxrQmhn/price-tag.jpg';

// FuncName().then(response => response.json().then(json => setStateName(json))).catch(err => console.log(err));

//#region DataController

export function GetAllStoresToDisplay() {
    return fetch(`https://localhost:5000/api/Data/GetAllStoresToDisplay`, {
        method: 'GET',
        // agent: httpsAgent,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    });
}

export function GetAllProductByStoreIDToDisplay(storeId) {
    return fetch(`https://localhost:5000/api/Data/GetAllProductByStoreIDToDisplay/${storeId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

//#endregion


//#region GuestUserController


/// Register new user to the system
/// Template of valid JSON:
/// {
///     "Email":"string",
///     "Password":"string"
/// }
export function Register( data ) {
    return fetch(`https://localhost:5000/api/GuestUser/Register`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}


/// search store by attributes
/// Template of valid JSON
/// {
///     "Name":"string",
///     "rating":double
/// }
/// NOTE: fields are optionals
export function SearchStore( search_by ) {    //TODO - search by other params
    return fetch(`https://localhost:5000/api/GuestUser/SearchStore`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { search_by },    
    })
}

/// Search product by attributes
/// Template of valid JSON:
/// {
///     "Name":"string",
///     "Category":"string",
///     "Lowprice":double,
///     "Highprice":double,
///     "Productrating":double,
///     "Storerating":double,
///     "Keywords":["string","string"]
/// }
/// NOTE: fields are optionals
export function SearchProduct( search_by ) {    //TODO - search by other params
    return fetch(`https://localhost:5000/api/GuestUser/SearchProduct`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { search_by },    
    })
}

/// Adding product to user's cart
/// Template of valid JSON:
/// {
///     "userID":"string",
///     "ProductID":"string",
///     "ProductQuantity":int,
///     "StoreID":"string"
/// }
export function AddProductToCart( data ) {    
    return fetch(`https://localhost:5000/api/GuestUser/AddProductToCart`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

/// Return the user's shopping cart
export function GetUserShoppingCart( userID ) {    
    return fetch(`https://localhost:5000/api/GuestUser/GetUserShoppingCart`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { userID },    
    })
}

/// Updates the user's shopping cart with the current quantity of a product
/// Template of valid JSON:
/// {
///     "userID":"string",
///     "shoppingBagID":"string",
///     "productID":"string",
///     "quantity":int
/// }
export function UpdateShoppingCart( data ) {    
    return fetch(`https://localhost:5000/api/GuestUser/UpdateShoppingCart`, {
        method: 'PUT',                                                                  //TODO -PUT
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

/// Preform purchase operation on the user shopping cart
/// Template of valid JSON:
/// {
///     "userID":"string",
///     "paymentDetails":{NOT IMPLEMENTED
///                         }
///     "deliveryDetails":{NOT IMPLEMENTED
///                         }
/// }
export function Purchase( data ) {    
    return fetch(`https://localhost:5000/api/GuestUser/Purchase`, {
        method: 'PUT',                                                      //TODO -PUT
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

/// Retrive the user purchase history
export function GetUserPurchaseHistory( userID ) {    
    return fetch(`https://localhost:5000/api/GuestUser/GetUserPurchaseHistory/${userID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { userID }, 
    })
}


/// Return the total amount of the user's shopping cart
export function GetTotalShoppingCartPrice( userID ) {    
    return fetch(`https://localhost:5000/api/GuestUser/GetTotalShoppingCartPrice/${userID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { userID }, 
    })
}





/// Returns all the reviews on a specific product
export function GetProductReview( data ) {    
    return fetch(`https://localhost:5000/api/GuestUser/GetProductReview/${data.storeID}/${data.productID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}





//#endregion


//#region RegisteredUserController

// Login to the system
// Template of valid JSON:
// {
//     "Email":"string",
//     "Password":"string"
// } 
export function Login( data ) {
    return fetch(`https://localhost:5000/api/RegisteredUser/Login`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

/// logout from the system
export function Logout( email ) {
    return fetch(`https://localhost:5000/api/RegisteredUser/Logout/${email}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { email },
    })
}

/// Open new store in the system
/// </summary>
/// Template of valid JSON:
/// {
///     "storeName":"string",
///     "userID":"string"
/// }
export function OpenNewStore( data) {
    return fetch(`https://localhost:5000/api/RegisteredUser/OpenNewStore`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}


/// Adding a new review on a product
/// Template of valid JSON:
/// {
///     "userID":"string",
///     "storeID":"string"
///     "productID":"string"
///     "review":"string"
/// }
export function AddProductReview( data ) {
    return fetch(`https://localhost:5000/api/RegisteredUser/AddProductReview`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

//#endregion


//#region StoreStaffController





//#endregion


//#region SystemAdminController





//#endregion













