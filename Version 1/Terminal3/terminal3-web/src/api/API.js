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
///
/// {
///   "message": "User shopping cart\n",
///   "execStatus": true,
///   "data": {
///     "id": "3404a9b11ab8435d8ae703effa1955ab",
///     "shoppingBags": [],
///     "totalCartPrice": 0
///    }
/// }
///
export function GetUserShoppingCart( userID ) {    
    return fetch(`https://localhost:5000/api/GuestUser/GetUserShoppingCart/${userID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        }, 
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


 /// Add product to store
/// Template of valid JSON:
/// {
///     "userID":"string",
///     "storeID":"string",
///     "productName":"string,
///     "price":double,
///     "initialQuantity":int,
///     "category":"string",
///     "keywords":["string","string"],
/// }
export function AddProductToStore( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/AddProductToStore`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

/// Removing product from store
/// <param name="userID">userId of the manager/owner who preform the action</param>
/// <param name="storeID">storeId where the product is located in</param>
/// <param name="productID">product identifier</param>
export function RemoveProductFromStore( userID, storeID, productID ) {
    return fetch(`https://localhost:5000/api/StoreStaff/RemoveProductFromStore/${userID}/${storeID}/${productID}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}


/// Edit product details
/// Template of valid JSON:
/// {
///     "userID":"string",
///     "storeID:"string",
///     "productID":"string",
///     "details": {
///                     "Name":"string",
///                     "Price":double,
///                     "Quantity":int,
///                     "Category":"string",
///                     "Keywords":["string","string"...]
///                 }
/// }
/// NOTE: all fields in "detalis" values are optionals
export function EditProductDetails( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/EditProductDetails`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

/// Add new store owner to a given store
/// Template of valid JSON:
/// {
///     "addedOwnerID":"string",
///     "currentlyOwnerID:"string",
///     "storeID":"string"
/// }
export function AddStoreOwner( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/AddStoreOwner`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}


/// Adding new store manager to a given store
/// Template of valid JSON:
/// {
///     "addedManagerID":"string",
///     "currentlyOwnerID:"string",
///     "storeID":"string"
/// }
export function AddStoreManager( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/AddStoreManager`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}


/// Setting new set of permissions to manager
/// Template of valid JSON:
/// {
///     "storeID":"string",
///     "managerID:"string",
///     "ownerID":"string",
///     "permissions":[int,int,int...]
/// }
export function SetPermissions( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/SetPermissions`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}


/// removing set of permissions to manager
/// Template of valid JSON:
/// {
///     "storeID":"string",
///     "managerID:"string",
///     "ownerID":"string",
///     "permissions":[int,int,int...]
/// }
export function RemovePermissions( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/RemovePermissions`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

/// Return list of pair, each pair hold details about the store staff and its permissions
/// <param name="ownerID">ID of the owner who request to preform the operation</param>
/// <param name="storeID">storeID</param>
export function GetStoreStaff( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/GetStoreStaff`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

/// Returns in-store purchase history
/// <param name="ownerID">ownerID</param>
/// <param name="storeID">ID of the store to get the purchase history</param>
export function GetStorePurchaseHistory( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/GetStorePurchaseHistory/${data.sysAdminID}/${data.storeId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}


/// Removing excisting store manager by an owner
/// <param name="storeID">StoreID</param>
/// <param name="currentlyOwnerID">OwnerID</param>
/// <param name="removedManagerID">ID of the manager to be removed</param>
export function RemoveStoreManager( storeID, currentlyOwnerID, removedManagerID ) {
    return fetch(`https://localhost:5000/api/StoreStaff/RemoveStoreManager/${storeID}/${currentlyOwnerID}/${removedManagerID}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}


//#endregion


//#region SystemAdminController

/// Returns user's purchase history
/// <param name="sysAdminID">system admin ID</param>
/// <param name="userID">ID of the user to get the purchase history</param>
export function AdminGetUserPurchaseHistory( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/SystemAdmin/GetUserPurchaseHistory/${data.sysAdminID}/${data.userID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}


/// Returns in-store purchase history
/// <param name="sysAdminID">system admin ID</param>
/// <param name="storeId">ID of the store to get the purchase history</param>
export function AdminGetStorePurchaseHistory( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/SystemAdmin/GetStorePurchaseHistory/${data.sysAdminID}/${data.storeID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}


/// Adding new system admin
/// <param name="sysAdminID">userId of the system admin who preform the addition</param>
/// <param name="email">email of the request new system admin</param>
export function AddSystemAdmin( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/SystemAdmin/AddSystemAdmin/${data.sysAdminID}/${data.email}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}


/// Removing existing system admin
/// <param name="sysAdminID">userId of the system admin who preform the addition</param>
/// <param name="email">email of admin to be removed</param>
export function RemoveSystemAdmin( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/SystemAdmin/RemoveSystemAdmin/${data.sysAdminID}/${data.email}`, {
        method: 'DELETE',                                       //TODO - HTTP DELETE
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}


/// Reset the system, including all the stored data
/// <param name="sysAdminID">userId of the system admin who preform the addition</param>
export function ResetSystem( data ) {
    return fetch(`https://localhost:5000/api/StoreStaff/SystemAdmin/ResetSystem/${data.sysAdminID}`, {
        method: 'POST',                                       
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}


//#endregion













