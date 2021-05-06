import React from 'react'
import { Typography, Button, Divider } from '@material-ui/core';
import { Elements, CardElement, ElementsConsumer } from '@stripe/react-stripe-js';
import { loadStripe } from '@stripe/stripe-js';

import Review from './Review'

// TODO: Fix payment to work without stripe
//       Figure what to do after pressing "Pay"
const stripePromise = loadStripe('...');

const PaymentForm = ({ shippingData, checkoutToken, nextStep, backStep }) => {
    const handleSubmit = (event, elements, stripe) => {
        event.preventDefault();     // prevent website from refreshing after clicking

        const orderData = {
            products: checkoutToken.products,
            customer: { firstname: shippingData.firstName, lastname: shippingData.lastName, email: shippingData.email },
            shipping: { 
                street: shippingData.address1, 
                city: shippingData.city, 
                zip: shippingData.zip,
                country: shippingData.shippingCountry 
            },
            payment: {
                //TODO: how to get payment data in a new way
            }
        }

        // TODO: do something with data

        nextStep();
    }

    return (
        <>
            <Review checkoutToken={checkoutToken} />
            <Divider />
            <Typography variant="h6" gutterBottom style={{ margin: '20px 0' }}>Payment Method</Typography>
            <Elements stripe={stripePromise}>
                <ElementsConsumer>
                    {({ elements, stripe }) => (
                        <form onSubmit={(e) => handleSubmit(e, elements, stripe)}>
                            <CardElement />
                            <br /> <br />
                            <div style={{ display: 'flex', justifyContent: 'space-between'}}>
                                <Button variant="outlined" onClick={backStep}>Back</Button>
                                <Button type="submit" variant="contained" disabled={!stripe} color="primary">
                                    Pay {checkoutToken.totalPrice}₪
                                </Button>
                            </div>
                        </form>
                    )}
                </ElementsConsumer>
            </Elements>
        </>
    )
}

export default PaymentForm;
