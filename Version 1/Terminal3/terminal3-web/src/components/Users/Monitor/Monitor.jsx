import React, { useState } from 'react'
import { Avatar, Button, CssBaseline, TextField, FormControlLabel, Checkbox, Link, Grid, Box, Typography, Container } from '@material-ui/core';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import { useHistory } from 'react-router-dom';
import {Action} from '../../../components';
import {LineChart} from '../../../components'
import  {createDataGroup} from '../../Charts/LineChart'

import useStyles from './styles';

function Copyright() {
    return (
      <Typography variant="body2" color="textSecondary" align="center">
        {'Copyright © '}
        <Link color="inherit" href="https://material-ui.com/">
          Terminal 3
        </Link>{' '}
        {new Date().getFullYear()}
        {'.'}
      </Typography>
    );
}


const Monitor = ({ userID }) => {
    // styles.js
    const classes = useStyles();
  
    // states
    const [data, setData] = useState({});

    const sample = createDataGroup('this is first name',['1','2','3','4','5','6','7'],[1,2,3,4,5,6,7]);
    const sample1 = createDataGroup('this is second name',['1','2','3','4','5','6','7'],[1,2,3,3,3,6,7]);
    //console.log(sample);

    // for redirecting after register
    let history = useHistory();

    const handleMonitor = (data) => {
        // const toSend = {startDate: data.startdate, endDate: data.enddate, storeID: store.id, ownerID: user.id};

        // GetIncomeAmountGroupByDay(toSend)
        //     .then(response => response.ok ?
        //         response.json().then(result => setIssued(true) & setInfo({data: result.data, type: 'incomes'})) : printErrorMessage(response)).catch(err => console.log(err));
    }

    const handleSubmit = (e) => {
        e.preventDefault();

        //handleRegister(data);

        // redirect to login page
        history.push('/login');
    }

    return (
    <Container component="main" maxWidth="xs">
        <CssBaseline />
            <div className={classes.paper}>
                <Avatar className={classes.avatar}> <LockOutlinedIcon /> </Avatar>
                <Typography component="h1" variant="h5"> Monitor System </Typography>
                <form className={classes.form} onSubmit={handleSubmit}>
                    <Grid container spacing={2}>
                        {/* <Grid item xs={12} sm={6}>
                        <TextField
                            autoComplete="fname"
                            name="firstName"
                            variant="outlined"
                            // required
                            fullWidth
                            id="firstName"
                            label="First Name"
                            autoFocus
                            onChange={(e) => setData({ ...data, firstname: e.target.value })}
                        />
                        </Grid>
                        <Grid item xs={12} sm={6}>
                        <TextField
                            variant="outlined"
                            // required
                            fullWidth
                            id="lastName"
                            label="Last Name"
                            name="lastName"
                            autoComplete="lname"
                            onChange={(e) => setData({ ...data, lastname: e.target.value })}
                        />
                        </Grid>
                        <Grid item xs={12}>
                        <TextField
                            variant="outlined"
                            required
                            fullWidth
                            id="email"
                            label="Email Address"
                            name="email"
                            autoComplete="email"
                            onChange={(e) => setData({ ...data, email: e.target.value })}
                        />
                        </Grid>
                        <Grid item xs={12}>
                        <TextField
                            variant="outlined"
                            required
                            fullWidth
                            name="password"
                            label="Password"
                            type="password"
                            id="password"
                            autoComplete="current-password"
                            onChange={(e) => setData({ ...data, password: e.target.value })}
                        />
                        </Grid>
                        <Grid item xs={12}>
                        <FormControlLabel
                            control={<Checkbox value="allowExtraEmails" color="primary" />}
                            label="I want to receive inspiration, marketing promotions and updates via email."
                        />
                        </Grid> */}
                        < LineChart DataGroups={[sample,sample1]} />
                    </Grid>
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        color="primary"
                        className={classes.submit}
                    >
                        Start live monitor
                    </Button>
                    <Action name='System Status by Day'
                            fields={[{name: 'Start Date', required: true, type: 'date'},
                                    {name: 'End Date', required: true, type: 'date'}]}   
                            handleAction={handleMonitor}/>
                </form>
            </div>
        <Box mt={5}>
            <Copyright />
        </Box>
    </Container>
    );
}

export default Monitor;
