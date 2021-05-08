import React from 'react';
import { AppBar, Toolbar, IconButton, Badge, MenuItem, Menu, Typography, Button } from '@material-ui/core';
import { ShoppingCart, LocalMall } from '@material-ui/icons';
import { Link, useLocation } from 'react-router-dom';

import useStyles from './styles';

import logo from '../../assets/terminal3_logo.png';

const Navbar = ( { id, totalItems, user, handleLogOut }) => {
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
                    {user.id !== -1 &&
                        <> 
                            <Typography variant="h6" color="primary">
                                Hello, {user.email.substr(0, user.email.indexOf('@'))}
                            </Typography>
                            <Button component={Link} to="/" className={classes.checkoutButton} size="large" 
                                    type="button" variant="text" color="primary" onClick={() => handleLogOut()}>
                                Sign Out
                            </Button>
                        </>
                    }
                    {user.id === -1 && location.pathname !== '/register' && (
                       <Button component={Link} to="/register" className={classes.checkoutButton} size="large" 
                                type="button" variant="text" color="primary">
                            Sign Up
                        </Button> 
                    ) }
                    {(user.id === -1 && location.pathname !== '/register' && location.pathname !== '/login') && <Typography> | </Typography>}
                    {user.id === -1 && location.pathname !== '/login' && (
                       <Button component={Link} to="/login" className={classes.checkoutButton} size="large" 
                                type="button" variant="text" color="primary">
                            Sign In
                        </Button>
                    ) }
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
