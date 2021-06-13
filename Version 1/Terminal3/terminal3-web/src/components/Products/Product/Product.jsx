import React, { useState, useEffect } from 'react'
import { Card, CardMedia, CardContent, CardActions, Typography, IconButton } from '@material-ui/core';
import { AddShoppingCart, AttachMoney, RateReview } from '@material-ui/icons';

import useStyles from './styles';
import { products_image_url } from '../../../api/API';
import { Offer } from '../../../components';

const Product = ({ product, onAddToBag, handleSendOfferToStore, handleGetProductReview }) => {
    const classes = useStyles();

    const [showOfferDialog, setShowOfferDialog] = useState(false);

    const onSendOffer = (data) => {
        const toSend = { ProductID: product.id, Amount: data.amount, Price: data.price };
        handleSendOfferToStore(toSend);
    };

    const onGetProductReview = (productID) => {
        handleGetProductReview({ productID });
    };

    return (
    <>
        <Card className={classes.root}>
            <CardMedia className={classes.media} image={products_image_url} title={product.name}/>
            <CardContent>
                <div className={classes.cardContent}>
                    <Typography variant="h5" gutterBottom>
                        {product.name}
                    </Typography>
                    <Typography variant="h5">
                        {product.price}₪
                    </Typography>
                </div>
                <Typography variant="subtitle2">    {/* TODO: Maybe somehow show only to staff */}
                    {product.id}
                </Typography>
                <Typography variant="body2" color="textSecondary">{product.category}</Typography>
            </CardContent>
            <CardActions disableSpacing className={classes.cardActions}>
                {onAddToBag !== null &&
                <>
                    <IconButton aria-label="Add to Bag" onClick={() => onAddToBag(product.id, product.name, product.price, 1, products_image_url)}>
                        <AddShoppingCart />
                    </IconButton>
                    <IconButton aria-label="Submit Offer" onClick={() => setShowOfferDialog(true)}>
                        <AttachMoney />
                    </IconButton>
                    <IconButton aria-label="Get Reviews" onClick={() => onGetProductReview(product.id)}>
                        <RateReview />
                    </IconButton>
                </>
                }
            </CardActions>
        </Card>
        {showOfferDialog && 
            <Offer type='new' setOpen={setShowOfferDialog} open={showOfferDialog} onSendOffer={onSendOffer} />
        }
    </>
    )
}

export default Product;
