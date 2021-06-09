export const products_image_url = 'https://i.ibb.co/HxrQmhn/price-tag.jpg';
const API_URL = 'https://localhost:5001/api';

export function printErrorMessage(response) {
    return response.json().then(message => alert(message));
}

//#region DataController

export function GetAllStoresToDisplay() {
    return fetch(`${API_URL}/Data/GetAllStoresToDisplay`, {
        method: 'GET',
        // agent: httpsAgent,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    });
}

export function GetAllProductByStoreIDToDisplay(storeId) {
    return fetch(`${API_URL}/Data/GetAllProductByStoreIDToDisplay/${storeId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

export function GetPermission(userId , storeId) {
    return fetch(`${API_URL}/Data/GetPermission/${userId}/${storeId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

//#endregion


//#region GuestUserController

/// Get welcome page of the system
export function EnterSystem() {
    return fetch(`${API_URL}/GuestUser`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}


/// Register new user to the system
/// Template of valid JSON:
/// {
///     "Email":"string",
///     "Password":"string"
/// }
export function Register( data ) {
    return fetch(`${API_URL}/GuestUser/Register`, {
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
export function SearchStore( search_by ) {
    return fetch(`${API_URL}/GuestUser/SearchStore`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(search_by),    
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
export function SearchProduct( search_by ) {
    return fetch(`${API_URL}/GuestUser/SearchProduct`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(search_by),    
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
    return fetch(`${API_URL}/GuestUser/AddProductToCart`, {
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
    return fetch(`${API_URL}/GuestUser/GetUserShoppingCart/${userID}`, {
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
    return fetch(`${API_URL}/GuestUser/UpdateShoppingCart`, {
        method: 'PUT',
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
    return fetch(`${API_URL}/GuestUser/Purchase`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

/// Retrive the user purchase history
export function GetUserPurchaseHistory( userID ) {    
    return fetch(`${API_URL}/GuestUser/GetUserPurchaseHistory/${userID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}


/// Return the total amount of the user's shopping cart
export function GetTotalShoppingCartPrice( userID ) {    
    return fetch(`${API_URL}/GuestUser/GetTotalShoppingCartPrice/${userID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}





/// Returns all the reviews on a specific product
export function GetProductReview( data ) {    
    return fetch(`${API_URL}/GuestUser/GetProductReview/${data.storeID}/${data.productID}`, {
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
    return fetch(`${API_URL}/RegisteredUser/Login`, {
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
    return fetch(`${API_URL}/RegisteredUser/Logout/${email}`, {
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
    return fetch(`${API_URL}/RegisteredUser/OpenNewStore`, {
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
    return fetch(`${API_URL}/RegisteredUser/AddProductReview`, {
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
    return fetch(`${API_URL}/StoreStaff/AddProductToStore`, {
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
    return fetch(`${API_URL}/StoreStaff/RemoveProductFromStore/${userID}/${storeID}/${productID}`, {
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
    return fetch(`${API_URL}/StoreStaff/EditProductDetails`, {
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
    return fetch(`${API_URL}/StoreStaff/AddStoreOwner`, {
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
    return fetch(`${API_URL}/StoreStaff/AddStoreManager`, {
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
    return fetch(`${API_URL}/StoreStaff/SetPermissions`, {
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
    return fetch(`${API_URL}/StoreStaff/RemovePermissions`, {
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
export function GetStoreStaff(ownerID, storeID) {
    return fetch(`${API_URL}/StoreStaff/GetStoreStaff/${ownerID}/${storeID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

/// Returns in-store purchase history
/// <param name="ownerID">ownerID</param>
/// <param name="storeID">ID of the store to get the purchase history</param>
export function GetStorePurchaseHistory( ownerID, storeID ) {
    return fetch(`${API_URL}/StoreStaff/GetStorePurchaseHistory/${ownerID}/${storeID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}


/// Removing existing store manager by an owner
/// <param name="storeID">StoreID</param>
/// <param name="currentlyOwnerID">OwnerID</param>
/// <param name="removedManagerID">ID of the manager to be removed</param>
export function RemoveStoreManager( storeID, currentlyOwnerID, removedManagerID ) {
    return fetch(`${API_URL}/StoreStaff/RemoveStoreManager/${storeID}/${currentlyOwnerID}/${removedManagerID}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

/// Removing existing store owner by the appointing owner
/// <param name="storeID">StoreID</param>
/// <param name="currentlyOwnerID">OwnerID</param>
/// <param name="removedOwnerID">ID of the owner to be removed</param>
export function RemoveStoreOwner( storeID, currentlyOwnerID, removedOwnerID ) {
    return fetch(`${API_URL}/StoreStaff/RemoveStoreOwner/${storeID}/${currentlyOwnerID}/${removedOwnerID}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

/// Get store's incomes by day
/// <param name="StartDate">Start Date</param>
/// <param name="EndDate">End Date</param>
/// <param name="storeID">StoreID</param>
/// <param name="OwnerID">OwnerID</param>
export function GetIncomeAmountGroupByDay( data ) {
    return fetch(`${API_URL}/StoreStaff/GetIncomeAmountGroupByDay`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

//#endregion


//#region SystemAdminController

/// Returns user's purchase history
/// <param name="sysAdminID">system admin ID</param>
/// <param name="userID">ID of the user to get the purchase history</param>
export function AdminGetUserPurchaseHistory( sysAdminID, userID ) {
    return fetch(`${API_URL}/SystemAdmin/GetUserPurchaseHistory/${sysAdminID}/${userID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}


/// Returns in-store purchase history
/// <param name="sysAdminID">system admin ID</param>
/// <param name="storeId">ID of the store to get the purchase history</param>
export function AdminGetStorePurchaseHistory( sysAdminID, storeID ) {
    return fetch(`${API_URL}/SystemAdmin/GetStorePurchaseHistory/${sysAdminID}/${storeID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}


/// Adding new system admin
/// <param name="sysAdminID">userId of the system admin who preform the addition</param>
/// <param name="email">email of the request new system admin</param>
export function AddSystemAdmin( sysAdminID, email ) {
    return fetch(`${API_URL}/SystemAdmin/AddSystemAdmin/${sysAdminID}/${email}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}


/// Removing existing system admin
/// <param name="sysAdminID">userId of the system admin who preform the addition</param>
/// <param name="email">email of admin to be removed</param>
export function RemoveSystemAdmin( sysAdminID, email ) {
    return fetch(`${API_URL}/SystemAdmin/RemoveSystemAdmin/${sysAdminID}/${email}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}


/// Reset the system, including all the stored data
/// <param name="sysAdminID">userId of the system admin who preform the addition</param>
export function ResetSystem( sysAdminID ) {
    return fetch(`${API_URL}/SystemAdmin/ResetSystem/${sysAdminID}`, {
        method: 'PUT',                                       
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

//#endregion



//#region Discount Policy

export function AddDiscountPolicy( data ) {
    return fetch(`${API_URL}/StoreStaff/AddDiscountPolicy`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

export function AddDiscountPolicyById( id, data ) {
    return fetch(`${API_URL}/StoreStaff/AddDiscountPolicy/${id}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

export function AddDiscountCondition( id, data ) {
    return fetch(`${API_URL}/StoreStaff/AddDiscountCondition/${id}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

export function RemoveDiscountPolicy( storeID, id ) {
    return fetch(`${API_URL}/StoreStaff/RemoveDiscountPolicy/${storeID}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

export function RemoveDiscountCondition( storeID, id ) {
    return fetch(`${API_URL}/StoreStaff/RemoveDiscountCondition/${storeID}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

export function GetDiscountPolicyData( storeID ) {
    return fetch(`${API_URL}/StoreStaff/GetDiscountPolicyData/${storeID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

//#endregion

//#region Purchase Policy

export function AddPurchasePolicy( data ) {
    return fetch(`${API_URL}/StoreStaff/AddPurchasePolicy`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

export function AddPurchasePolicyById( id, data ) {
    return fetch(`${API_URL}/StoreStaff/AddPurchasePolicy/${id}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    })
}

export function RemovePurchasePolicy( storeID, id ) {
    return fetch(`${API_URL}/StoreStaff/RemovePurchasePolicy/${storeID}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

export function GetPurchasePolicyData( storeID ) {
    return fetch(`${API_URL}/StoreStaff/GetPurchasePolicyData/${storeID}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

//#endregion