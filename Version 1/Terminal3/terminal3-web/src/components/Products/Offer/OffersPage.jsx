import React, { useState, useEffect } from 'react';
import { List, ListItem, ListItemText, Typography, Paper, Button, Divider } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

import { Offer } from '../../../components';
import { printErrorMessage, GetStoreOffers, GetUserOffers, SendOfferResponseToUser } from '../../../api/API';

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

const OffersPage = ({ type, storeID, userID }) => {
    const classes = useStyles();

    // Offers list data from API
    const [data, setData] = useState(null);
    const [forceRender, setForceRender] = useState(0);

    // Counter offer
    const [selectedOffer, setSelectedOffer] = useState(null);
    const [showCounterDialog, setShowCounterDialog] = useState(false);

    // TODO: Update on click
    const fetchStoreOffers = async () => {
        GetStoreOffers({ StoreID: storeID }).then(response => response.ok ?
            response.json().then(result => setData(result.data)) : printErrorMessage(response)).catch(err => console.log(err));
    };

    const fetchUserOffers = async () => {
        GetUserOffers({ UserID: userID }).then(response => response.ok ?
            response.json().then(result => setData(result.data)) : printErrorMessage(response)).catch(err => console.log(err)); 
    }

    // TODO: Connect to API
    const handleAccept = (offerID, customerID) => {
        if (type === 'store'){
            const toSend = { StoreId: storeID, OwnerID: userID, UserID: customerID, OfferID: offerID, Accepted: true }
            SendOfferResponseToUser(toSend).then(response => response.ok ?
                response.json().then(result => alert(result.message)) : printErrorMessage(response)).catch(err => console.log(err));
        }
        else if (type === 'user')
            console.log("ACCEPTED BY USER");
    };

    // TODO: Connect to API
    const handleDecline = (offerID, customerID) => {
        if (type === 'store'){
            const toSend = { StoreId: storeID, OwnerID: userID, UserID: customerID, OfferID: offerID, Accepted: false, CounterOffer: -1 }
            SendOfferResponseToUser(toSend).then(response => response.ok ?
                response.json().then(result => alert(result.message)) : printErrorMessage(response)).catch(err => console.log(err));
            setForceRender(prev => prev + 1);
        }
        else if (type === 'user')
            console.log("DECLINED BY USER");
    };

    const onCounter = (offerID, customerID) => {
        setSelectedOffer({ offerID, customerID })
        setShowCounterDialog(true);
    };

    const handleCounter = (counterOffer) => {
        const toSend = { StoreID: storeID, OwnerID: userID, UserID: selectedOffer.customerID, OfferID: selectedOffer.offerID, Accepted: false, CounterOffer: counterOffer.price }
        SendOfferResponseToUser(toSend).then(response => response.ok ?
            response.json().then(result => alert(result.message)) : printErrorMessage(response)).catch(err => console.log(err));
    };

    useEffect(() => {
        if (type === 'store')
            fetchStoreOffers();
        else if (type === 'user')
            fetchUserOffers();
    }, []);

    useEffect(() => {
        if (type === 'store')
            fetchStoreOffers();
        else if (type === 'user')
            fetchUserOffers();
    }, [forceRender]);

    useEffect(() => {
        console.log(data);
    }, [data]);

    return (
        <div className={classes.root}>
            <div className={classes.toolbar} />
            <main className={classes.layout}>
                <Paper className={classes.paper}>
                    <List component="nav" aria-label="offers list">
                        {(data === null || data.length === 0) && (
                            <Typography variant="body2">No pending offers.</Typography>
                        )}
                        {(type === 'store' && data !== null) && (
                            data.map((offer) => (
                            <>
                                <ListItem style={{padding: '10px 0'}} key={offer.Id}>
                                    <ListItemText primary={offer.Product} secondary={`User: ${offer.User}`} />
                                    <Typography variant="body2">{`Amount: ${offer.Amount}, Price: ${offer.Price}₪`}</Typography>
                                </ListItem>
                                <Button color="primary" onClick={() => handleAccept(offer.Id, offer.User)}>Accept</Button>
                                <Button color="primary" onClick={() => onCounter(offer.Id, offer.User)}>Counter</Button>
                                <Button color="secondary" onClick={() => handleDecline(offer.Id, offer.User)}>Decline</Button>
                                <Divider />
                            </>
                            ))
                        )}

                        {(type === 'user' && data !== null) && (
                            data.map((offer) => (
                            <>
                                <ListItem style={{padding: '10px 0'}} key={offer.Id}>
                                    <ListItemText primary={offer.Product} secondary={`Store: ${offer.Store}`} />
                                    <Typography variant="body2">{`Amount: ${offer.Amount}, Price: ${offer.Price}₪`}</Typography>
                                </ListItem>
                                {offer.CounterOfferPrice !== -1 ? (
                                <>
                                    <Button color="primary" onClick={() => handleAccept(offer.Id)}>Accept</Button>
                                    <Button color="secondary" onClick={() => handleDecline(offer.Id)}>Decline</Button>
                                </>
                                )
                                :
                                (<Typography variant="body2">PENDING</Typography>)
                                }
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
