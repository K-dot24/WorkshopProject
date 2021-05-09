import React, { useState } from 'react'
import { Avatar, Button, CssBaseline, TextField, FormControlLabel, Checkbox, Link, Grid, Box, Typography, Container } from '@material-ui/core';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import { Receipt } from '@material-ui/icons';
import { useHistory, useLocation } from 'react-router-dom';

import useStyles from './styles';

const Action = ({ name, fields, handleAction }) => {
     // styles.js
     const classes = useStyles();
    
     // states
     const [data, setData] = useState({});

     const location = useLocation();
     let history = useHistory();    // for redirecting after login
 
     // TODO: fetch user from API
     const handleSubmit = (e) => {
         e.preventDefault();

         console.log(data);
 
         handleAction(data);
 
         // redirect back to homepage
        //  history.push('/');
     }

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline />
                <div className={classes.paper}>
                    <Avatar className={classes.avatar}>
                        <Receipt />
                    </Avatar>
                    <Typography component="h1" variant="h5">
                        {name}
                    </Typography>
                    <form className={classes.form} onSubmit={handleSubmit}>
                        {fields.map((field) => (
                            <TextField
                            key={field}
                            variant="outlined"
                            margin="normal"
                            required
                            fullWidth
                            id={field}
                            label={field}
                            name={field}
                            autoComplete={field}
                            autoFocus
                            onChange={(e) => setData({ ...data, [field.replace(/\s/g, "").toLowerCase()]: e.target.value })}
                        />
                        ))}
                        <Button
                            type="submit"
                            fullWidth
                            variant="contained"
                            color="primary"
                            className={classes.submit}
                        >
                            Submit
                        </Button>
                    </form>
                </div>
        </Container>
    )
}

export default Action;
