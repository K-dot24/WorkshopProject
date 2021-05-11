import React, { useState } from 'react'
import { Avatar, Button, CssBaseline, TextField, FormControlLabel, Checkbox, Link, Grid, Box, Typography, Container } from '@material-ui/core';
import { Receipt } from '@material-ui/icons';
import { useHistory, useLocation } from 'react-router-dom';

import useStyles from './styles';
import { CheckboxList } from '../../../components';

const Action = ({ name, fields, handleAction }) => {
     // styles.js
     const classes = useStyles();
    
     // states
     const [data, setData] = useState({});
     const [checkBoxArray, setCheckBoxArray] = useState([]);

     const location = useLocation();
     let history = useHistory();    // for redirecting after login
 
     // TODO: fetch user from API
     const handleSubmit = (e) => {
         e.preventDefault();

         if (name === 'Set Permissions') {
            handleAction(checkBoxArray, data);
         }
         else {
            // console.log(data);
            handleAction(data);
         }
 
         // redirect back to homepage
        //  history.push('/');
     }

     const handleCheckBox = (checkList) => {
        setCheckBoxArray(checkList);
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
                    {fields && 
                        fields.map((field) => (
                            <TextField
                                key={field.name}
                                type={field.type ? field.type : "text"}
                                variant="outlined"
                                margin="normal"
                                required={field.required}
                                fullWidth
                                id={field.name}
                                label={field.name}
                                name={field.name}
                                autoComplete={field.name}
                                autoFocus
                                onChange={(e) => setData({ ...data, [field.name.replace(/\s/g, "").toLowerCase()]: (field.type && field.type === "number") ? parseInt(e.target.value) : e.target.value })}
                            />
                        ))
                    }
                    {name === 'Set Permissions' && (<CheckboxList handleCheckBox={handleCheckBox} />)}
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        color="primary"
                        className={classes.submit}
                    >
                        OK
                    </Button>
                </form>
                    
                </div>
        </Container>
    )
}

export default Action;
