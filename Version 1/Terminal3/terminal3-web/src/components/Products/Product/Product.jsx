import React from 'react'
import { Card, CardMedia, CardContent, CardActions, Typography, IconButton } from '@material-ui/core';
import { AddShoppingCart } from '@material-ui/icons';

import useStyles from './styles';

// TODO: Check properties' names after getting real products
//       Check onAddToCart after fetching real data
const Product = ({ product, onAddToBag }) => {
    const classes = useStyles();

    // TODO: Change onAddToCart to any quantity
    return (
        <Card className={classes.root}>
            <CardMedia className={classes.media} image={product.image} title={product.name}/>
            <CardContent>
                <div className={classes.cardContent}>
                    <Typography variant="h5" gutterBottom>
                        {product.name}
                    </Typography>
                    <Typography variant="h5">
                        {product.price}₪
                    </Typography>
                </div>
                <Typography variant="body2" color="textSecondary">{product.category}</Typography>
            </CardContent>
            <CardActions disableSpacing className={classes.cardActions}>
                <IconButton aria-label="Add to Bag" onClick={() => onAddToBag(product.id, product.name, product.price, 1, product.image)}>
                    <AddShoppingCart />
                </IconButton>
            </CardActions>
        </Card>
    )
}

export default Product;
