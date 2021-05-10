import React from 'react';
import { AppBar, Toolbar, IconButton, Badge, MenuItem, Menu, Typography, Button } from '@material-ui/core';
import { ShoppingCart, LocalMall, Menu as MenuIcon, Notifications as NotificationsIcon } from '@material-ui/icons';
import { Link, useLocation, useHistory } from 'react-router-dom';

import { GetPermission } from '../../api/API';

import useStyles from './styles';

import logo from '../../assets/terminal3_logo.png';

const Navbar = ( { storeId, totalItems, user, handleLogOut }) => {
    const classes = useStyles();
    const location = useLocation();
    let history = useHistory();

    const allActions = ['Add New Product', 'Remove Product', 'Edit Product Details', 'Add Store Owner', 'Add Store Manager', 'Remove Store Manager', 
                        'Set Permissions', 'Get Store Staff', 'Set Purchase Policy At Store', 'Get Purchase Policy At Store',
                        'Set Discount Policy At Store', 'Get Discount Policy At Store', 'Get Store Purchase History'];
    
    // TODO: Delete this when connecting to real API permissions
    const userWithMockPermissions = { ...user, permissions: [true, true, true, true, true, true, true, true, true, true, true, true, true]};

    const fetchPermissions = async () => {
        GetPermission(user.id, storeId).then(response => response.json().then(json => console.log(json))).catch(err => console.log(err));
    }


    if (allActions.length !== userWithMockPermissions.permissions.length)
        console.log("Unmatching number of actions and permissions");

    const [anchorEl, setAnchorEl] = React.useState(null);
    const isMenuOpen = Boolean(anchorEl);

    const StoreActions = () => {    // TODO: Get real permissions from API
        return [
            allActions.map((action, index) => userWithMockPermissions.permissions[index] &&  
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
                <MenuItem onClick={() => handleMenuClick(`/${user.id}/purchase_history`)}>Purchase History</MenuItem>
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
                    {user.id !== -1 &&
                        <Typography variant="h6" color="primary">
                                    Hello, {user.email.substr(0, user.email.indexOf('@'))}
                        </Typography>
                    }
                    <div className={classes.grow} />
                    {user.id !== -1 &&
                        <> 
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
                    {user.id !== -1 &&
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
