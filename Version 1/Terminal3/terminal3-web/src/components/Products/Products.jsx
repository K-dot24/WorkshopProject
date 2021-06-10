import React from 'react';
import { Grid, Typography } from '@material-ui/core';

import Product from './Product/Product';
import useStyles from './styles';

const Products = ({ storeName, products, onAddToBag, handleSendOfferToStore }) => {
    const classes = useStyles();

    return (
        <main className={classes.content}>
            <div className={classes.toolbar} />

            {/* Title */}
            <Typography className={classes.title} variant="h3" color="primary" gutterBottom>
                {storeName !== 'SEARCH_RES' ? `Welcome to ${storeName}` : `Search Results`}
            </Typography>
            
            {/* Products */}
            <Grid container justify="center" spacing={4}>
                {storeName !== 'SEARCH_RES' ?
                    products.map((product) => (
                        <Grid item key={product.id} xs={12} sm={6} md={4} lg={3}>
                            <Product product={product} onAddToBag={onAddToBag} 
                                    handleSendOfferToStore={handleSendOfferToStore} 
                            />
                        </Grid>
                    )) 
                :
                    products.map((product) => (
                        <Grid item key={product.id} xs={12} sm={6} md={4} lg={3}>
                            <Product product={product} onAddToBag={null} />
                        </Grid>
                    ))   
                }
            </Grid>
        </main>
    );
}

export default Products;