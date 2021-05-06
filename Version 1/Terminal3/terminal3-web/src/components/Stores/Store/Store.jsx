import React from 'react'
import { Card, CardMedia, CardContent, CardActions, Typography, IconButton } from '@material-ui/core';
import { Storefront } from '@material-ui/icons';
import { Link } from 'react-router-dom';

import useStyles from './styles';

// TODO: Make whole card linkable to store page
const Store = ({ store }) => {
    const classes = useStyles();

    return (
        <Link to={{pathname: `/stores/${store.id}`, state: { store: store }}} style={{ textDecoration: "none" }}>
        <Card className={classes.root}>
            <CardMedia className={classes.media} image={store.image} title={store.name} alt={store.name}/>
            <CardContent>
                <div className={classes.cardContent}>
                    <Typography variant="h5" gutterBottom>
                        {store.name}
                    </Typography>
                    <Typography variant="h5" color="textSecondary">
                        {store.rating}★
                    </Typography>
                </div>
                <Typography variant="body2" color="textSecondary">{store.category}</Typography>
            </CardContent>
            <CardActions disableSpacing className={classes.cardActions}>
                <IconButton component={Link} to={{pathname: `/stores/${store.id}`, state: { store: store }}} aria-label="Open store page">
                    <Storefront />
                </IconButton>
            </CardActions>
        </Card>
        </Link>
    )
}

export default Store;
