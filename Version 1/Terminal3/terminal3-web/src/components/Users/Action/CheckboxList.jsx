import React, { useState, useEffect } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { List, ListItem, ListItemIcon, ListItemSecondaryAction, ListItemText, Checkbox, Button,
        Avatar, CssBaseline, TextField, Typography, Container, FormControl, Select, InputLabel, FormHelperText } from '@material-ui/core';
import { Receipt } from '@material-ui/icons';

import useStyles from './styles';

const permissions_array = ['Add New Product', 'Remove Product', 'Edit Product Details', 'Add Store Owner', 'Add Store Manager',
'Remove Store Manager', 'Set Permissions', 'Get Store Staff Details', 'Set Purchase Policy', 'Get Purchase Policy',
'Set Discount Policy', 'Get Discount Policy', 'Get Store Purchase History'];

const CheckboxList = ({ handleSetPermissions }) => {
    const classes = useStyles();
    const [checked, setChecked] = useState([]);

    const [data, setData] = useState({});

    const handleToggle = (value) => () => {
        const currentIndex = checked.indexOf(value);
        const newChecked = [...checked];

        if (currentIndex === -1) {
        newChecked.push(value);
        } else {
        newChecked.splice(currentIndex, 1);
        }

        setChecked(newChecked);
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        handleSetPermissions(checked, data);
    }

    return (

        <Container component="main" maxWidth="xs">
            <CssBaseline />
            <div className={classes.paper}>
                <Avatar className={classes.avatar}>
                    <Receipt />
                </Avatar>
                <Typography component="h1" variant="h5">
                    Set Permissions
                </Typography>
                <form className={classes.form} onSubmit={handleSubmit}>
                <TextField
                    type="text"
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    id="Manager ID"
                    label="Manager ID"
                    name="Manager ID"
                    autoComplete="Manager ID"
                    autoFocus
                    onChange={(e) => setData({ managerid: e.target.value })}
                />
                <List className={classes.root}>
                    {permissions_array.map((value, index) => {
                        const labelId = `checkbox-list-label-${value}`;

                        return (
                            <ListItem key={index} role={undefined} dense button onClick={handleToggle(index)}>
                                <ListItemIcon>
                                <Checkbox
                                    edge="start"
                                    checked={checked.indexOf(index) !== -1}
                                    tabIndex={-1}
                                    disableRipple
                                    inputProps={{ 'aria-labelledby': labelId }}
                                />
                                </ListItemIcon>
                                <ListItemText id={labelId} primary={`${value}`} />
                                <ListItemSecondaryAction>
                                </ListItemSecondaryAction>
                            </ListItem>
                        );
                    })}
                </List>
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

export default CheckboxList;
