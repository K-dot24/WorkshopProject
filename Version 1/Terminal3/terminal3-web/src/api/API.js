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
export function Register(email, password) {
    return fetch(`https://localhost:5000/api/GuestUser/Register`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { email, password },
    })
}


/// search store by attributes
/// Template of valid JSON
/// {
///     "Name":"string",
///     "rating":double
/// }
/// NOTE: fields are optionals
export function SearchStore() {
    return fetch(`https://localhost:5000/api/GuestUser/SearchStore`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { Name, rating },    //TODO
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
export function Login(email, password) {
    return fetch(`https://localhost:5000/api/RegisteredUser/Login`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { email, password },
    })
}

export function Logout(email) {
    return fetch(`https://localhost:5000/api/RegisteredUser/Logout/${email}`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { password },
    })
}


/// Open new store in the system
/// </summary>
/// Template of valid JSON:
/// {
///     "storeName":"string",
///     "userID":"string"
/// }
export function OpenNewStore(email) {
    return fetch(`https://localhost:5000/api/RegisteredUser/OpenNewStore`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { storeName, userID },
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
export function AddProductReview(email) {
    return fetch(`https://localhost:5000/api/RegisteredUser/AddProductReview`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { userID, storeID, productID, review },
    })
}

//#endregion


//#region StoreStaffController





//#endregion


//#region SystemAdminController





//#endregion





// export async function GetAllStoresToDisplay() {
//     const response = await fetch('https://localhost:5000/api/Data/GetAllStoresToDisplay');
//     const json = await response.json();

//     console.log(json);
//     return json;
// }








