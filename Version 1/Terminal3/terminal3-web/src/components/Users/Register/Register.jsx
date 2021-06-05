import React, { useState } from 'react'
import { Avatar, Button, CssBaseline, TextField, FormControlLabel, Checkbox, Link, Grid, Box, Typography, Container } from '@material-ui/core';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import { useHistory } from 'react-router-dom';

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

const Register = ({ handleRegister }) => {
    // styles.js
    const classes = useStyles();
    
    // states
    const [data, setData] = useState({});

    // for redirecting after register
    let history = useHistory();

    const handleSubmit = (e) => {
        e.preventDefault();

        handleRegister(data);

        // redirect to login page
        history.push('/login');
    }

    return (
    <Container component="main" maxWidth="xs">
        <CssBaseline />
            <div className={classes.paper}>
                <Avatar className={classes.avatar}>
                    <LockOutlinedIcon />
                </Avatar>
                <Typography component="h1" variant="h5">
                    Sign up
                </Typography>
                <form className={classes.form} onSubmit={handleSubmit}>
                    <Grid container spacing={2}>
                        <Grid item xs={12} sm={6}>
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
                        </Grid>
                    </Grid>
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        color="primary"
                        className={classes.submit}
                    >
                        Sign Up
                    </Button>
                    <Grid container justify="flex-end">
                        <Grid item>
                        <Link href="/login" variant="body2">
                            Already have an account? Sign in
                        </Link>
                        </Grid>
                    </Grid>
                </form>
            </div>
        <Box mt={5}>
            <Copyright />
        </Box>
    </Container>
    );
}

export default Register;
