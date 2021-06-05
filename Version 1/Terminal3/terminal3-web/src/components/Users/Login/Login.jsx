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

const Login = ({ handleLogin }) => {
    // styles.js
    const classes = useStyles();
    
    // states
    const [user, setUser] = useState({email: '', password: ''});

    // for redirecting after login
    let history = useHistory();

    const handleSubmit = (e) => {
        e.preventDefault();

        handleLogin(user);

        // redirect back to homepage
        history.push('/');
    }

    // const submit = e => {
    //     e.preventDefault()
    //     fetch('/api', {
    //       method: 'POST',
    //       body: JSON.stringify({ user }),
    //       headers: { 'Content-Type': 'application/json' },
    //     })
    //       .then(res => res.json())
    //       .then(json => setUser(json.user))
    // }

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline />
                <div className={classes.paper}>
                    <Avatar className={classes.avatar}>
                        <LockOutlinedIcon />
                    </Avatar>
                    <Typography component="h1" variant="h5">
                        Sign in
                    </Typography>
                    <form className={classes.form} onSubmit={handleSubmit}>
                        <TextField
                            variant="outlined"
                            margin="normal"
                            required
                            fullWidth
                            id="email"
                            label="Email Address"
                            name="email"
                            autoComplete="email"
                            autoFocus
                            onChange={(e) => setUser({ ...user, email: e.target.value })}
                        />
                        <TextField
                            variant="outlined"
                            margin="normal"
                            required
                            fullWidth
                            name="password"
                            label="Password"
                            type="password"
                            id="password"
                            autoComplete="current-password"
                            onChange={(e) => setUser({ ...user, password: e.target.value })}
                        />
                        <FormControlLabel
                            control={<Checkbox value="remember" color="primary" />}
                            label="Remember me"
                        />
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            color="primary"
                            className={classes.submit}
                        >
                            Sign In
                        </Button>
                        <Grid container>
                            <Grid item xs>
                            {/* <Link href="#" variant="body2">
                                Forgot password?
                            </Link> */}
                            </Grid>
                            <Grid item>
                            <Link href="/register" variant="body2">
                                {"Don't have an account? Sign Up"}
                            </Link>
                            </Grid>
                        </Grid>
                    </form>
                </div>
            <Box mt={8}>
                <Copyright />
            </Box>
        </Container>
  );
}

export default Login;
