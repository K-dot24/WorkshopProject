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

// export async function GetAllStoresToDisplay() {
//     const response = await fetch('https://localhost:5000/api/Data/GetAllStoresToDisplay');
//     const json = await response.json();

//     console.log(json);
//     return json;
// }

export function GetAllProductByStoreIDToDisplay(storeId) {
    return fetch(`http://localhost:5000/api/Data/GetAllProductByStoreIDToDisplay/${storeId}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
    })
}

export function Register(email, password) {
    return fetch(`http://localhost:5000/api/GuestUser/Register`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { email, password },
    })
}

export function Login(email, password) {
    return fetch(`http://localhost:5000/api/GuestUser/Login`, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: { email, password },
    })
}



