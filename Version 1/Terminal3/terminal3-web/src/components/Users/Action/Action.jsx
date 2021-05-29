import React, { useState, useEffect } from 'react'
import { Avatar, Button, CssBaseline, TextField, FormControlLabel, Checkbox, Link, Grid, Box, Typography, Container,
        FormControl, Select, InputLabel, FormHelperText } from '@material-ui/core';
import { Receipt } from '@material-ui/icons';
import { useHistory, useLocation } from 'react-router-dom';

import useStyles from './styles';
import { CheckboxList } from '../../../components';

const Action = ({ name, fields, handleAction, mainTypes, subTypes }) => {
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
            if (currentType.value === '')
                handleAction(data);
            else
                handleAction(data, currentType.value);
         }
 
         // redirect back to homepage
        //  history.push('/');
     }

    const handleCheckBox = (checkList) => {
        setCheckBoxArray(checkList);
    }

    const [currentType, setCurrentType] = useState({ value: '', sub: '' });
    
    const handleChange = (event) => {
        const name = event.target.name;
        setCurrentType({
            ...currentType,
            [name]: event.target.value,
        });
    };

    useEffect(() => {
        console.log(currentType);
    }, [currentType]);

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

                    {/* Type Dropdown Menu (for policies) */}
                    {mainTypes &&
                        (
                        <>
                        <FormControl className={classes.formControl}>
                            <InputLabel htmlFor="value-native-simple">Type</InputLabel>
                            <Select
                                native
                                required={true}
                                value={currentType.value}
                                onChange={handleChange}
                                inputProps={{
                                    name: 'value',
                                    id: 'value-native-simple',
                                }}
                            >
                            <option aria-label="None" value="" />
                            {mainTypes.map((type, index) => (
                                <option key={index} value={type.name}>{type.name}</option>
                            ))}
                            </Select>
                        </FormControl>
                        <FormHelperText>Choose type of discount</FormHelperText>
                        </>
                        )
                    }

                    {subTypes && (
                        subTypes.map((subType) => subType.main === currentType.value && (
                        <>
                            <FormControl className={classes.formControl}>
                                <InputLabel htmlFor="sub-native-simple"></InputLabel>
                                <Select
                                    native
                                    required={true}
                                    value={currentType.sub}
                                    onChange={handleChange}
                                    inputProps={{
                                        name: 'sub',
                                        id: 'sub-native-simple',
                                    }}
                                >
                                <option aria-label="None" value="" />
                                {subType.subs.map((type, index) => (
                                    <option key={index} value={type.name}>{type.name}</option>
                                ))}
                                </Select>
                            </FormControl>
                            <FormHelperText>Choose type of discount target</FormHelperText>
                        </>
                        ))
                    )}

                    {/* Text Fields */}
                    {fields && 
                        fields.map((field) => (
                            'belongsTo' in field && (currentType.value !== field.belongsTo && currentType.sub !== field.belongsTo) ? null :
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
