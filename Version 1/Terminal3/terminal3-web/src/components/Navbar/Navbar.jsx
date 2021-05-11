import React, { useState, useEffect } from 'react';
import { AppBar, Toolbar, IconButton, Badge, MenuItem, Menu, Typography, Button } from '@material-ui/core';
import { ShoppingCart, LocalMall, Menu as MenuIcon, Notifications as NotificationsIcon } from '@material-ui/icons';
import { Link, useLocation, useHistory } from 'react-router-dom';

import { GetPermission } from '../../api/API';

import useStyles from './styles';

import logo from '../../assets/terminal3_logo.png';

const Navbar = ( { storeId, totalItems, user, handleLogOut }) => {
    const [permissions, setPermissions] = useState([])
    
    const classes = useStyles();
    const location = useLocation();

    let history = useHistory();

    const allActions = ['Add New Product', 'Remove Product', 'Edit Product Details', 'Add Store Owner', 'Add Store Manager', 'Remove Store Manager', 
                        'Set Permissions', 'Get Store Staff', 'Set Purchase Policy At Store', 'Get Purchase Policy At Store',
                        'Set Discount Policy At Store', 'Get Discount Policy At Store', 'Get Store Purchase History'];
    
    // TODO: Delete this when connecting to real API permissions
    const userWithMockPermissions = { ...user, permissions: [true, true, true, true, true, true, true, true, true, true, true, true, true]};

    const fetchPermissions = async () => {
        if (user.loggedIn && storeId !== -1) {
            GetPermission(user.id, storeId).then(response => response.ok ? 
                response.json().then(permissions => setPermissions(permissions)) : null).catch(err => console.log(err));
        }
    }


    if (allActions.length !== userWithMockPermissions.permissions.length)
        console.log("Unmatching number of actions and permissions");

    const [anchorEl, setAnchorEl] = React.useState(null);
    const isMenuOpen = Boolean(anchorEl);

    const StoreActions = () => {    // TODO: Get real permissions from API
        return [
            allActions.map((action, index) => permissions[index] &&  
                        <MenuItem key={index} onClick={() => handleMenuClick(`/stores/${storeId}/${action.replace(/\s/g, "").toLowerCase()}`)}>{action}</MenuItem>    
            )
        ];
    };

    const handleMenuOpen = (event) => {
        setAnchorEl(event.currentTarget);
      };

    const handleMenuClick = (path) => {
        history.push(path);
        setAnchorEl(null);
    };

    useEffect(() => {
        fetchPermissions();
        console.log(user);
    }, [user]);

    const menuId = 'primary-search-account-menu';
    const renderMenu = (
        <Menu
        anchorEl={anchorEl}
        anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
        id={menuId}
        keepMounted
        transformOrigin={{ vertical: 'top', horizontal: 'right' }}
        open={isMenuOpen}
        onClose={() => setAnchorEl(null)}
        >
        {storeId === -1 ? (
            <div>
                <MenuItem onClick={() => handleMenuClick(`/${user.id}/openstore`)}>Open New Store</MenuItem>
                <MenuItem onClick={() => handleMenuClick(`/${user.id}/review`)}>Write Review</MenuItem>
                <MenuItem onClick={() => handleMenuClick(`/${user.id}/purchasehistory`)}>Purchase History</MenuItem>
                {/* <MenuItem onClick={() => handleMenuClick(`/${user.id}/addsystemadmin`)}>Add System Admin</MenuItem>
                <MenuItem onClick={() => handleMenuClick(`/${user.id}/removesystemadmin`)}>Remove System Admin</MenuItem>
                <MenuItem onClick={() => handleMenuClick(`/${user.id}/resetsystem`)}>Reset System</MenuItem> */}
            </div>
        ) : (
            <StoreActions />
        )
        
        }
        </Menu>
    );
    
    return (
        <>
            <AppBar position="fixed" className={classes.appBar} color="inherit">
                <Toolbar>
                    <Typography component={Link} to="/" variant="h6" className={classes.title} color="inherit">
                        <img src={logo} alt="Terminal 3" height="50px" className={classes.image} />
                    </Typography>
                    {user.loggedIn &&
                        <Typography variant="h6" color="primary">
                                    Hello, {user.email.substr(0, user.email.indexOf('@'))}
                        </Typography>
                    }
                    <div className={classes.grow} />
                    {user.loggedIn &&
                        <> 
                            <Button component={Link} to="/" className={classes.checkoutButton} size="large" 
                                    type="button" variant="text" color="primary" onClick={() => handleLogOut()}>
                                Sign Out
                            </Button>
                        </>
                    }
                    {!user.loggedIn && location.pathname !== '/register' && (
                       <Button component={Link} to="/register" className={classes.checkoutButton} size="large" 
                                type="button" variant="text" color="primary">
                            Sign Up
                        </Button> 
                    ) }
                    {(!user.loggedIn && location.pathname !== '/register' && location.pathname !== '/login') && <Typography> | </Typography>}
                    {!user.loggedIn && location.pathname !== '/login' && (
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
                        (location.pathname.includes(`/stores/${storeId}`) && storeId !== -1 ) && (
                            <div className={classes.button}>
                            <IconButton component={Link} to={`/stores/${storeId}/bag`} aria-label="Show bag items" color="inherit">
                                <Badge badgeContent={totalItems} color="secondary">
                                    <LocalMall />
                                </Badge>
                            </IconButton>
                        </div>)
                    }
                    <div className={classes.sectionDesktop}>
                        <IconButton aria-label="show 2 new notifications" color="inherit">
                            <Badge badgeContent={2} color="secondary">
                                <NotificationsIcon />
                            </Badge>
                        </IconButton>
                    </div>
                    {(user.loggedIn) &&
                        <IconButton edge="start" className={classes.menuButton} color="inherit" aria-label="open drawer" onClick={handleMenuOpen}>
                            <MenuIcon />
                        </IconButton>
                    }
                </Toolbar>
            </AppBar>
            {renderMenu}
        </>
    )
}

export default Navbar;
