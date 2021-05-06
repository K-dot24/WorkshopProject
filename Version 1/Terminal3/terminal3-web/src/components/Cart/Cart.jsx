import React from 'react'
import { Container, Typography, Button, Grid } from '@material-ui/core';
import { Link } from 'react-router-dom';

import useStyles from './styles';
import CartItem from './CartItem/CartItem';

const Cart = ({ id, cart, handleUpdateCartQuantity, handleRemoveFromCart, handleEmptyCart, handleAddToCart }) => {
    const classes = useStyles();

    const EmptyCart = () => (
        id === 0 ? 
            <Typography variant="subtitle1">You have no items in your shopping cart. Go shopping!
                <Link to="/" className={classes.link}> Start adding some products</Link>
            </Typography> 
        :
            <Typography variant="subtitle1">You have no items in your shopping bag. Why not add some?
            <Link to={`/stores/${id}`} className={classes.link}> Start adding some products</Link>
            </Typography>
    );
    const FilledCart = () => (
        <>
            <Grid container spacing={3}>
                {cart.products.map((item) => (
                    <Grid item xs={12} sm={4} key={item.id}>
                        <CartItem item={item} handleUpdateCartQuantity={handleUpdateCartQuantity} handleRemoveFromCart={handleRemoveFromCart} />
                    </Grid>
                ))}
            </Grid>
            {id === 0 ? (
                <div className={classes.cardDetails}>
                        <Typography variant="h4">Subtotal: { cart.totalPrice }₪</Typography>
                        <div>
                            <Button className={classes.emptyButton} size="large" type="button" variant="contained" 
                                    color="secondary" onClick={handleEmptyCart}>
                                Empty Cart
                            </Button>
                            <Button component={Link} to="/checkout" className={classes.checkoutButton} size="large" 
                                    type="button" variant="contained" color="primary">
                                Checkout
                            </Button>
                        </div>
                </div>) 
            : 
                (<div className={classes.cardDetails}>
                    <Typography variant="h4">Subtotal: { cart.totalPrice }₪</Typography>
                    <div>
                        <Button component={Link} to={`/stores/${id}`} className={classes.backButton} 
                                size="large" type="button" variant="contained" color="inherit">
                            Back to Shop
                        </Button>
                        <Button className={classes.emptyButton} size="large" type="button" variant="contained" 
                                color="secondary" onClick={handleEmptyCart}>
                            Empty Bag
                        </Button>
                        <Button component={Link} to="/cart" className={classes.checkoutButton} 
                                size="large" type="button" variant="contained" color="primary"
                                onClick={() => cart.products.map((item) => (
                                    handleAddToCart(item.id, item.name, item.price, item.quantity, item.image)
                                ))} >
                            Add to Cart
                        </Button>
                    </div>
                </div>)
            }
        </>
    );

    // if (!cart.products.length) return 'Loading...';

    return (
        <Container>
            <div className={classes.toolbar}/>
            <Typography className={classes.title} variant="h3" gutterBottom>{id === 0 ? "Your Shopping Cart" : "Your Shopping Bag"}</Typography>
            { !cart.products.length ? <EmptyCart /> : <FilledCart /> }
        </Container>
    )
}

export default Cart;
