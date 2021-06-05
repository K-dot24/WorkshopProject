import React, { useState } from 'react'
import { Typography, Button, Divider } from '@material-ui/core';

import CreditCardInput from 'react-credit-card-input';

import Review from './Review'
import { printErrorMessage, Purchase } from '../../api/API';

const PaymentForm = ({ userID, shippingData, checkoutToken, nextStep, backStep }) => {
    const [cardNumber, setCardNumber] = useState('');
    const [expiry, setExpiry] = useState('');
    const [cvc, setCvc] = useState('');

    const handleCardNumberChange = (data) => {
        setCardNumber(data);
    }

    const handleExpiryChange = (data) => {
        setExpiry(data);
    }
    
    const handleCvcChange = (data) => {
        setCvc(data);
    }

    const handleSubmit = (event) => {
        event.preventDefault();     // prevent website from refreshing after clicking

        const fullName = shippingData.firstname + ' ' + shippingData.lastname;

        // Split expiry date to month and yearh
        const splited = expiry.split('/');
        const month = splited[0].replace(/\b0+/g, '');
        let year = '20' + splited[1];
        year = year.replace(' ', '');

        const orderData = {
            // products: checkoutToken.products,
            // customer: { firstname: shippingData.firstName, lastname: shippingData.lastName, email: shippingData.email },
            userID: userID,
            deliveryDetails: {
                name: fullName,
                address: shippingData.address, 
                city: shippingData.city, 
                country: shippingData.shippingCountry,
                zip: shippingData.zip
            },
            paymentDetails: {
                card_number: cardNumber.replace(/ /g,''),
                month,
                year,
                holder: fullName,
                ccv: cvc,
                id: userID
            }
        }

        Purchase(orderData).then(response => response.ok ?
            response.json().then(result => result.execStatus ? nextStep() : console.log(result.message)) : printErrorMessage(response)).catch(err => console.log(err));
    }

    return (
        <>
            <Review checkoutToken={checkoutToken} />
            <Divider />
            <Typography variant="h6" gutterBottom style={{ margin: '20px 0' }}>Payment Method</Typography>
            <form onSubmit={(e) => handleSubmit(e)}>
                <CreditCardInput
                    cardNumberInputProps={{ value: cardNumber, onChange: ((e) => handleCardNumberChange(e.target.value)) }}
                    cardExpiryInputProps={{ value: expiry, onChange: ((e) => handleExpiryChange(e.target.value)) }}
                    cardCVCInputProps={{ value: cvc, onChange: ((e) => handleCvcChange(e.target.value)) }}
                    fieldClassName="input"
                />
                <br /> <br />
                <div style={{ display: 'flex', justifyContent: 'space-between'}}>
                    <Button variant="outlined" onClick={backStep}>Back</Button>
                    <Button type="submit" variant="contained" color="primary">
                        Pay {checkoutToken.totalPrice}₪
                    </Button>
                </div>
            </form>
        </>
    )
}

export default PaymentForm;
