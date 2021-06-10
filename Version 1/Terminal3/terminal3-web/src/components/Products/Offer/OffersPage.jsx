import React, { useState, useEffect } from 'react';
import { List, ListItem, ListItemText, Typography, Paper, Button, Divider } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

import { Offer } from '../../../components';
import { GetStoreOffersData } from '../../../api/API';

const useStyles = makeStyles((theme) => ({
    root: {
        height: 110,
        flexGrow: 1,
        maxWidth: 400,
    },
    toolbar: theme.mixins.toolbar,
    layout: {
        marginTop: '5%',
        width: 'auto',
        marginLeft: theme.spacing(2),
        marginRight: theme.spacing(2),
        [theme.breakpoints.up(600 + theme.spacing(2) * 2)]: {
            width: 600,
            marginLeft: 'auto',
            marginRight: 'auto',
        },
    },
    paper: {
        marginTop: theme.spacing(3),
        marginBottom: theme.spacing(3),
        padding: theme.spacing(2),
        [theme.breakpoints.down('xs')]: {
            width: '100%',
            marginTop: 60,
        },
        [theme.breakpoints.up(600 + theme.spacing(3) * 2)]: {
            marginTop: theme.spacing(6),
            marginBottom: theme.spacing(6),
            padding: theme.spacing(3),
        },
    },
}));

const OffersPage = ({ storeID, userID }) => {
    const classes = useStyles();

    // Offers list data from API
    const [data, setData] = useState(null);

    // Counter offer
    const [selectedOfferID, setSelectedOfferID] = useState(null);
    const [showCounterDialog, setShowCounterDialog] = useState(false);

    // TODO: Update with real API call
    const fetchOffers = () => {
        const res = GetStoreOffersData(userID, storeID);
        setData(res);
    };

    // TODO: Connect to API
    const handleAccept = (offerID) => {
        const toSend = { StoreId: storeID, UserId: userID, OfferId: offerID, Accepted: true }
        console.log(toSend);
    };

    // TODO: Connect to API
    const handleDecline = (offerID) => {
        const toSend = { StoreId: storeID, UserId: userID, OfferId: offerID, Accepted: false, CounterOffer: -1 }
        console.log(toSend);
    };


    const onCounter = (offerID) => {
        setSelectedOfferID(offerID)
        setShowCounterDialog(true);
    };

    // TODO: Connect to API
    const handleCounter = (counterOffer) => {
        const toSend = { StoreId: storeID, UserId: userID, OfferId: selectedOfferID, Accepted: false, CounterOffer: counterOffer.price }
        console.log(toSend);
    };

    useEffect(() => {
        fetchOffers();
    }, []);

    useEffect(() => {
        console.log(data);
    }, [data]);

    // TODO: Check real data properties
    return (
        <div className={classes.root}>
            <div className={classes.toolbar} />
            <main className={classes.layout}>
                <Paper className={classes.paper}>
                    <List component="nav" aria-label="offers list">
                        {data !== null && (
                            data.offers.map((offer) => (
                            <>
                                <ListItem style={{padding: '10px 0'}} key={offer.offerID}>
                                    <ListItemText primary={offer.productName} secondary={`User: ${offer.userID}`} />
                                    <Typography variant="body2">{`Amount: ${offer.amount}, Price: ${offer.price}₪`}</Typography>
                                </ListItem>
                                <Button color="primary" onClick={() => handleAccept(offer.offerID)}>Accept</Button>
                                <Button color="primary" onClick={() => onCounter(offer.offerID)}>Counter</Button>
                                <Button color="secondary" onClick={() => handleDecline(offer.offerID)}>Decline</Button>
                                <Divider />
                            </>
                            ))
                        )}
                    </List>
                </Paper>
            </main>
            {showCounterDialog && 
                <Offer type='counter' setOpen={setShowCounterDialog} open={showCounterDialog} onSendOffer={handleCounter} />
            }
        </div>
    )
}

export default OffersPage;
