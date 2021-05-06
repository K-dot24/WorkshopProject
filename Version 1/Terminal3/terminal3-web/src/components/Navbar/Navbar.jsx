import React from 'react';
import { AppBar, Toolbar, IconButton, Badge, MenuItem, Menu, Typography } from '@material-ui/core';
import { ShoppingCart, LocalMall } from '@material-ui/icons';
import { Link, useLocation } from 'react-router-dom';

import logo from '../../assets/terminal3_logo.png';
import useStyles from './styles';

const Navbar = ( { id, totalItems }) => {
    const classes = useStyles();
    const location = useLocation();
    
    return (
        <>
            <AppBar position="fixed" className={classes.appBar} color="inherit">
                <Toolbar>
                    <Typography component={Link} to="/" variant="h6" className={classes.title} color="inherit">
                        <img src={logo} alt="Terminal 3" height="50px" className={classes.image} />
                    </Typography>
                    <div className={classes.grow} />
                    {location.pathname === '/' ? (
                        <div className={classes.button}>
                            <IconButton component={Link} to="/cart" aria-label="Show cart items" color="inherit">
                                <Badge badgeContent={totalItems} color="secondary">
                                    <ShoppingCart />
                                </Badge>
                            </IconButton>
                        </div>) :
                        (location.pathname.includes(`/stores/`) && id !== -1 ) && (
                            <div className={classes.button}>
                            <IconButton component={Link} to={`/stores/${id}/bag`} aria-label="Show bag items" color="inherit">
                                <Badge badgeContent={totalItems} color="secondary">
                                    <LocalMall />
                                </Badge>
                            </IconButton>
                        </div>)
                    }
                </Toolbar>
            </AppBar>
        </>
    )
}

export default Navbar;
