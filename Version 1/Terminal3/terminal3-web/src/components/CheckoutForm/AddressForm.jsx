import React, { useState, useEffect } from 'react'
import { InputLabel, Select, MenuItem, Button, Grid, Typography } from '@material-ui/core';
import { useForm, FormProvider } from 'react-hook-form';
import { Link } from 'react-router-dom';

import FormInput from './FormInput';


const AddressForm = ({ next }) => {
    const [shippingCountries, setShippingCountries] = useState([]);
    const [shippingCountry, setShippingCountry] = useState('');
    const [data, setData] = useState({});

    const methods = useForm();

    // TODO: Fetch from api?
    //     : checkoutToken?
    const fetchShippingCountries = async () => {
        const countries = ['Israel', 'Cyprus'];
        setShippingCountries(countries);
        setShippingCountry(countries[0]);
    }

    const handleSubmit = (e) => {
        // e.preventDefault();
        next({ ...data, shippingCountry });
    }

    useEffect(() => {
        fetchShippingCountries();
    }, []);

    return (
        <>
            <Typography variant="h6" gutterBottom>Shipping Address</Typography>
            <FormProvider {...methods}>
                <form onSubmit={handleSubmit}>
                    <Grid container spacing={3}>
                        <FormInput name='firstName' label='First Name' onChange={(e) => setData({ ...data, firstname: e.target.value })} />
                        <FormInput name='lastName' label='Last Name' onChange={(e) => setData({ ...data, lastname: e.target.value })} />
                        <FormInput name='address1' label='Address' onChange={(e) => setData({ ...data, address: e.target.value })} />
                        <FormInput name='city' label='City' onChange={(e) => setData({ ...data, city: e.target.value })} />
                        <FormInput name='zip' label='ZIP / Postal Code' onChange={(e) => setData({ ...data, zip: e.target.value })} />
                        <FormInput name='email' label='Email' onChange={(e) => setData({ ...data, email: e.target.value })} />
                        <Grid item xs={12} sm={6}>
                            <InputLabel>Shipping Country</InputLabel>
                            <Select value={shippingCountry} fullWidth onChange={(e) => setShippingCountry(e.target.value)}>
                                {shippingCountries.map(name => (
                                    <MenuItem key={name} value={name}>
                                        {name}
                                    </MenuItem>
                                ))}
                            </Select>
                        </Grid>
                    </Grid>
                    <br/>
                    <div style={{display: 'flex', justifyContent: 'space-between'}}>
                            <Button component={Link} to="/cart" variant="outlined">Back to Cart</Button>
                            <Button type="submit" variant="contained" color="primary">Next</Button>
                    </div>
                </form>
            </FormProvider>
        </>
    )
}

export default AddressForm;
