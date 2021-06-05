import React from 'react'
import { Typography, List, ListItem, ListItemText, Paper, Divider } from '@material-ui/core';
import { useLocation } from 'react-router-dom';

import useStyles from './styles'; 

const Review = ({ checkoutToken }) => {
    const location = useLocation();
    const classes = useStyles();

    return (
        <>
            <Typography variant="h6" gutterBottom>Order Summary</Typography>
            {location.pathname === '/checkout' ? (
                <List disablePadding>
                {checkoutToken.products.map((product) => (
                    <ListItem style={{padding: '10px 0'}} key={product.name}>
                        <ListItemText primary={product.name} secondary={`Quantity: ${product.quantity}`} />
                        <Typography variant="body2">{product.price * product.quantity}₪</Typography>
                    </ListItem>
                ))}
                <ListItem style={{padding: '10px 0'}}>
                    <ListItemText primary="Total" />
                    <Typography variant="subtitle1" style={{ fontWeight: 700 }}>
                        {checkoutToken.totalPrice}₪
                    </Typography>
                </ListItem>
            </List>  
            ) : (
            <>
                <div className={classes.toolbar} />
                <main className={classes.layout}>
                    <Paper className={classes.paper}>
                        <Typography variant="h6" gutterBottom>Purchase History</Typography>
                        <List disablePadding>
                            {checkoutToken && checkoutToken.shoppingBags.map((bag) => bag.products.map((product) => (
                                <ListItem style={{padding: '10px 0'}} key={product.item1.name}>
                                    <ListItemText primary={product.item1.name} secondary={`Quantity: ${product.item2}`} />
                                    <Typography variant="body2">{product.item1.price * product.item2}₪</Typography>
                                </ListItem>
                            )))}
                        </List>
                    </Paper>
                </main>                  
            </>  
            )}
        </>
    )
}

export default Review
