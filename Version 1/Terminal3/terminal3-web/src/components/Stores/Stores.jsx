import React, { useState, useEffect } from 'react'
import { Grid } from '@material-ui/core';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
 
import { Store, StorePage } from '../../components';
import { GetAllStoresToDisplay } from '../../api/API';
import useStyles from './styles';

const Stores = ({ match, user, handleAddToCart, handleLogOut }) => {
    const [stores, setStores] = useState([]);
    console.log(match);

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
        <div>
            <Switch>
                <Route exact path={match.url}>
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
                </Route>
                <Route path={match.url + "stores/:id"} 
                        render={({match}) => (<StorePage store={stores.find(s => s.id === match.params.id)} user={user} handleAddToCart={handleAddToCart} handleLogOut={handleLogOut} match={match} />)} />
            </Switch>
        </div>
    );
}

export default Stores;
