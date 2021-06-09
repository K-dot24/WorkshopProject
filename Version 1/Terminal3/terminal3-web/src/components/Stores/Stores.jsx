import React, { useState, useEffect } from 'react'
import { Grid } from '@material-ui/core';
import { BrowserRouter as Router, Switch, Route, useLocation } from 'react-router-dom';
 
import { Store, StorePage } from '../../components';
import { GetAllStoresToDisplay, SearchStore } from '../../api/API';
import useStyles from './styles';

const Stores = ({ setPathname, match, user, searchQuery, handleAddToCart, handleLogOut, isSystemAdmin }) => {
    const [stores, setStores] = useState([]);

    const classes = useStyles();
    const location = useLocation();

    const fetchStores = async () => {
        GetAllStoresToDisplay().then(response => response.json().then(json => setStores(json))).catch(err => console.log(err));
    }

    const searchStoresByQuery = async () => {
        const query = {Name: searchQuery};
        console.log(query);
        SearchStore(query).then(response => response.json().then(json => json.execStatus ? setStores(json.data) : alert(json.message))).catch(err => console.log(err));
    }

    // On first render
    useEffect(() => { 
        fetchStores();
    }, []);
    
    // On search query update
    useEffect(() => {
        if (searchQuery !== '')
            searchStoresByQuery();
        else
            fetchStores();
    }, [searchQuery]);

    useEffect(() => {
        setPathname(location);
    }, [location]);

    return (
        <div>
            <Switch>
                <Route exact path={match.url}>
                    <main className={classes.content}>
                        <div className={classes.toolbar} />
                        <Grid container justify="center" spacing={4}>
                            {stores.map((store) => (
                                <Grid item key={store.id} xs={12} sm={6} md={4} lg={3}>
                                    <Store store={store} isSystemAdmin={isSystemAdmin} />
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
