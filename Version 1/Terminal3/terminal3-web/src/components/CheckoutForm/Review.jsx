import React, { useState } from 'react'
import { Typography, List, ListItem, ListItemText, Paper, Divider, Button } from '@material-ui/core';
import { useLocation } from 'react-router-dom';

import { printErrorMessage, AddProductReview } from '../../api/API';
import useStyles from './styles';
import UserReviewDialog from './UserReviewDialog';

const Review = ({ checkoutToken }) => {
    const location = useLocation();
    const classes = useStyles();

    // User Review
    const [reviewDetails, setReviewDetails] = useState(null);
    const [showReviewDialog, setShowReviewDialog] = useState(false);

    const onReview = (userID, storeID, productID) => {
        setReviewDetails({ userID, storeID, productID })
        setShowReviewDialog(true);
    };
    
    const handleAddReview = (review) => {
        const toSend = { ...reviewDetails, ...review }
        AddProductReview(toSend).then(response => response.ok ?
            response.json().then(message => alert(message)) : printErrorMessage(response)).catch(err => console.log(err));
    };

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
                        {checkoutToken && checkoutToken.shoppingBags.map(function(bag){
                                let toRender = bag.products.map((product) => (
                                    <div key={product.item1.id}>
                                        <ListItem style={{padding: '10px 0'}} key={product.item1.id}>
                                            <ListItemText primary={product.item1.name} secondary={`Quantity: ${product.item2}`} />
                                            {/* <Typography variant="body2">{product.item1.price * product.item2}₪</Typography> */}
                                        <Button color="primary" onClick={() => onReview(bag.userId, bag.storeId, product.item1.id)}>Add Review</Button>
                                        </ListItem>
                                    </div>
                                ));

                                toRender.push(
                                    <div>
                                        <Divider />
                                        <Typography variant="body2">{`Total Bag Price: ${bag.totalBagPrice}₪`}</Typography>
                                        <Divider />
                                    </div>
                                )
                                
                                return toRender;
                            }
                            )}
                        </List>
                    </Paper>
                </main>
                {showReviewDialog && 
                    <UserReviewDialog setOpen={setShowReviewDialog} open={showReviewDialog} onAddReview={handleAddReview} />
                } 
            </>  
            )}
        </>
    )
}

export default Review
