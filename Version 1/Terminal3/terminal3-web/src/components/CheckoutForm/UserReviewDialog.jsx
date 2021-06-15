import React, { useState } from 'react';
import { Button, Dialog, TextField, DialogActions, DialogContent, DialogContentText, DialogTitle } from '@material-ui/core';

const UserReviewDialog = ({ open, setOpen, onAddReview }) => {
    const [data, setData] = useState(null);

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleSubmit = () => {
        onAddReview(data);
        handleClose();
    };

    return (
        <div>
            <Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Product Review</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Please enter your review:
                    </DialogContentText>
                    <TextField
                        required={true}
                        autoFocus
                        margin="dense"
                        id="review"
                        label="Review"
                        type="text"
                        fullWidth
                        onChange={(e) => setData({ ...data, review: e.target.value })}
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

export default UserReviewDialog;
