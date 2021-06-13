import React from 'react';
import { Typography, Avatar, ListItemText, ListItem, List, Paper } from '@material-ui/core'
import { Receipt } from '@material-ui/icons';

import useStyles from './styles';

const InfoDisplay = ({ info }) => {
    const classes = useStyles();

    return (
        <>
        <div className={classes.toolbar} />
        <main className={classes.layout}>
            <div className={classes.title}>
                <Avatar className={classes.avatar}>
                    <Receipt />
                </Avatar>
                <Typography variant="h6" gutterBottom>{info.type === 'productReview' ? 'Reviews' : 'Results'}</Typography>
            </div>
            <Paper className={classes.paper}>
                <List disablePadding>
                    {info.type === 'purchaseHistory' ? (
                        info.data.shoppingBags.map((bag) => bag.products.map((product) => (
                            <ListItem style={{padding: '10px 0'}} key={product.item1.name}>
                                <ListItemText primary={product.item1.name} secondary={`User: ${bag.userId} | Quantity: ${product.item2}`} />
                                <Typography variant="body2">{product.item1.price * product.item2}₪</Typography>
                            </ListItem>
                        )))
                    )
                    :
                    info.type === 'productReview' ? info.data.length > 0 ? (
                        info.data.map((review, index) => (
                            <ListItem style={{padding: '10px 0'}} key={index}>
                                <ListItemText primary={review.item2} secondary={`User: ${review.item1}`} />
                                {/* <Typography variant="body2">{product.item1.price * product.item2}₪</Typography> */}
                            </ListItem>
                        ))
                    ) : (<Typography variant="body2">No available reviews on this product at the moment.</Typography>)
                    :
                    info.data.map((item, index) => ( info.type === 'incomes' ?
                        <ListItem style={{padding: '10px 0'}} key={index}>
                            <ListItemText primary={item.item1.substring(0, 10)} />
                            <Typography variant="body2">{item.item2}₪</Typography>
                        </ListItem>
                    :
                        info.type === 'staff' &&
                        <ListItem style={{padding: '10px 0'}} key={index}>
                            <ListItemText primary={item.item1.id} secondary={item.item2.isOwner ? "Owner" : "Manager"} />
                            <Typography variant="body2">{`Permissions: ${item.item2.functionsBitMask.map((permission, index) => (
                                permission ? index : null
                            ))}`}</Typography>
                        </ListItem>
                    ))}
                </List>
            </Paper>
        </main>
    </>
    )
}

export default InfoDisplay;
