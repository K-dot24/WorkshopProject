import React from 'react'
import { Typography, Button, Card, CardActions, CardContent, CardMedia } from '@material-ui/core';

import useStyles from './styles';
import { products_image_url } from '../../../api/API';

const CartItem = ({ item, handleUpdateCartQuantity }) => {
    const classes = useStyles();

    return (
        <Card>
            <CardMedia image={products_image_url} alt={item.name} className={classes.media} />
            <CardContent className={classes.cardContent}>
                <Typography variant="h5">{item.name}</Typography>
                <Typography variant="h6">{item.price * item.quantity}₪</Typography>
            </CardContent>
            <CardActions className={classes.cardActions}>
                <div className={classes.buttons}>
                    <Button type="button" size="small" onClick={() => handleUpdateCartQuantity(item.storeID, item.id, item.quantity - 1)}>-</Button>
                    <Typography>{item.quantity}</Typography>
                    <Button type="button" size="small" onClick={() => handleUpdateCartQuantity(item.storeID, item.id, item.quantity + 1)}>+</Button>
                </div>
                <Button variant="contained" type="button" color="secondary" onClick={() => handleUpdateCartQuantity(item.storeID, item.id, 0)}>Remove</Button>
            </CardActions>
        </Card>
    )
}

export default CartItem;
