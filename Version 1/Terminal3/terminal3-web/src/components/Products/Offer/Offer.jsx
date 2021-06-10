import React, { useState } from 'react';
import { Button, Dialog, TextField, DialogActions, DialogContent, DialogContentText, DialogTitle } from '@material-ui/core';

const Offer = ({ type, open, setOpen, onSendOffer }) => {
    const [data, setData] = useState(null);

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleSubmit = () => {
        onSendOffer(data);
        handleClose();
    };

    return (
        <div>
            <Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Submit Offer</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        {type === 'new' ? 'To submit an offer, please enter the following details:' 
                                        : 'To submit a counter offer, please enter the following details:'}
                    </DialogContentText>
                    {type === 'new' && (
                        <TextField
                            required={true}
                            autoFocus
                            margin="dense"
                            id="amount"
                            label="Amount"
                            type="number"
                            fullWidth
                            onChange={(e) => setData({ ...data, amount: parseInt(e.target.value) })}
                        />
                    )}
                    <TextField
                        required={true}
                        autoFocus={type === 'counter'}
                        margin="dense"
                        id="price"
                        label="Offered Price"
                        type="number"
                        fullWidth
                        onChange={(e) => setData({ ...data, price: parseFloat(e.target.value) })}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose} color="primary">
                        Cancel
                    </Button>
                    <Button onClick={handleSubmit} color="primary">
                        Submit
                    </Button>
                </DialogActions>
            </Dialog>
        </div>
    )
}

export default Offer;
