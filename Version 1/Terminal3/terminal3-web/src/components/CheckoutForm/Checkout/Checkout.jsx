import React, { useState, useEffect } from 'react'
import { Paper, Stepper, Step, StepLabel, Typography, CircularProgress, Divider, Button } from '@material-ui/core';
import { Link } from 'react-router-dom';

import { printErrorMessage, Purchase } from '../../../api/API';
import useStyles from './styles';
import AddressForm from '../AddressForm';
import PaymentForm from '../PaymentForm';

const steps = ['Shipping Address', 'Payment Details'];

const Checkout = ({ userID, cart, setUpdateCart }) => {
    const [activeStep, setActiveStep] = useState(0);
    const [checkoutToken, setCheckoutToken] = useState(null);
    const [shippingData, setShippingData] = useState({});
    const [isFinished, setIsFinished] = useState(false);
    
    const classes = useStyles();

    const handlePurchase = (orderData) => {
        nextStep();

        Purchase(orderData).then(response => response.ok ?
            response.json().then(result => result.execStatus ? setIsFinished(true) : console.log(result.message)) : printErrorMessage(response)).catch(err => console.log(err));
    }

    useEffect(() => {
        const generateToken = async () => {
            try {
                const token = { products: cart.products, totalPrice: cart.totalPrice };
                setCheckoutToken(token);
            } catch (error) {
                
            }
        }

        generateToken();
    }, [cart])

    const nextStep = () => setActiveStep((prevActiveState) => prevActiveState + 1);
    const backStep = () => setActiveStep((prevActiveState) => prevActiveState - 1);

    const next = (data) => {
        setShippingData(data);
        nextStep();
    }

    const Confirmation = () => !isFinished ? (
        <>
            {/* {timeout()} */}
            <div className={classes.spinner}>
                <CircularProgress />
            </div>
        </>
    ) : (
        <>
            <div>
                <Typography>Thank you for your purchase!</Typography>
                <Divider className={classes.divider} />
                {/* <Typography variant="subtitle2">Order ref: ref</Typography> */}
            </div>
            <br />
            <Button component={Link} to="/" variant="outlined" type="button" onClick={() => setUpdateCart(true)}>Take me back</Button>
        </>
    );

    const Form = () => activeStep === 0 ?
        <AddressForm next={next} /> :
        <PaymentForm userID={userID} shippingData={shippingData} checkoutToken={checkoutToken} 
                    nextStep={nextStep} backStep={backStep} handlePurchase={handlePurchase} />

    return (
        <>
            <div className={classes.toolbar} />
            <main className={classes.layout}>
                <Paper className={classes.paper}>
                    <Typography variant="h4" align="center">Checkout</Typography>
                    <Stepper activeStep={activeStep} className={classes.stepper}>
                        {steps.map((step) => (
                            <Step key={step}>
                                <StepLabel>{step}</StepLabel>
                            </Step>
                        ))}
                    </Stepper>
                    {activeStep === steps.length ? <Confirmation /> : checkoutToken && <Form />}
                </Paper>
            </main>  
        </>
    )
}

export default Checkout;
