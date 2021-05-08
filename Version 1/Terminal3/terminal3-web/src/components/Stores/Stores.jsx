import React, { useState, useEffect } from 'react'
import { Grid } from '@material-ui/core';
import Axios from 'axios';
import axios from 'axios';
 
import { GetAllStoresToDisplay } from '../../api/API';
import Store from './Store/Store';
import useStyles from './styles';

const Stores = () => {
    const [stores, setStores] = useState([]);

    const classes = useStyles();

    const fetchStores = async () => {
        GetAllStoresToDisplay().then(response => response.json().then(json => setStores(json))).catch(err => console.log(err));

        // Mock data
        // setStores([
        //     { id: 1, name: 'Topshop', rating: 4, image: 'https://salience.co.uk/wp-content/uploads/topshop-1.jpg'},
        //     { id: 2, name: 'ZARA MEN', rating: 4.5, image: 'https://blackfriday-best.co.il/wp-content/uploads/2020/11/ZARA-logo.jpg'}
        // ]);
    }

    useEffect(() => { 
        fetchStores();
    }, []);  

    return (
        <main className={classes.content}>
            <div className={classes.toolbar} />
            <Grid container justify="center" spacing={4}>
                {stores.map((store) => (
                    <Grid item key={store.id} xs={12} sm={6} md={4} lg={3}>
                        <Store store={store} />
                    </Grid>
                ))}
            </Grid>
        </main>
    );
}

export default Stores;
