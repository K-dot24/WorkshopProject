import React, { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import ListItemText from '@material-ui/core/ListItemText';
import Checkbox from '@material-ui/core/Checkbox';
import IconButton from '@material-ui/core/IconButton';
import CommentIcon from '@material-ui/icons/Comment';

const useStyles = makeStyles((theme) => ({
    root: {
      width: '100%',
      maxWidth: 360,
      backgroundColor: theme.palette.background.paper,
    },
  }));

const CheckboxList = ({ handleCheckBox }) => {
    const classes = useStyles();
    const [checked, setChecked] = React.useState([0]);
    const [checkList, setCheckList] = useState([]);

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

    const handleCheck = (event, isInputChecked, value) => {
        switch (value) {
            case 'Add New Product':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 0]);
                else
                    setCheckList(checkList.filter(p => p !== 0));
                break;
            case 'Remove Product':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 1]);
                else
                    setCheckList(checkList.filter(p => p !== 1));
                break;
            case 'Edit Product Details':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 2]);
                else
                    setCheckList(checkList.filter(p => p !== 2));
                break;
            case 'Add Store Owner':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 3]);
                else
                    setCheckList(checkList.filter(p => p !== 3));
                break;
            case 'Add Store Manager':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 4]);
                else
                    setCheckList(checkList.filter(p => p !== 4));
                break;
            case 'Remove Store Manager':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 5]);
                else
                    setCheckList(checkList.filter(p => p !== 5));
                break;
            case 'Set Permissions':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 6]);
                else
                    setCheckList(checkList.filter(p => p !== 6));
                break;
            case 'Get Store Staff Details':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 7]);
                else
                    setCheckList(checkList.filter(p => p !== 7));
                break;
            case 'Set Purchase Policy':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 8]);
                else
                    setCheckList(checkList.filter(p => p !== 8));
                break;
            case 'Get Purchase Policy':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 9]);
                else
                    setCheckList(checkList.filter(p => p !== 9));
                break;
            case 'Set Discount Policy':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 10]);
                else
                    setCheckList(checkList.filter(p => p !== 10));
                break;
            case 'Get Discount Policy':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 11]);
                else
                    setCheckList(checkList.filter(p => p !== 11));
                break;
            case 'Get Store Purchase History':
                if (isInputChecked)
                    setCheckList((prevState) => [...prevState, 12]);
                else
                    setCheckList(checkList.filter(p => p !== 12));
                break;
        }

        handleCheckBox(checkList);
    };

    return (
        <List className={classes.root}>
            {['Add New Product', 'Remove Product', 'Edit Product Details', 'Add Store Owner', 'Add Store Manager',
            'Remove Store Manager', 'Set Permissions', 'Get Store Staff Details', 'Set Purchase Policy', 'Get Purchase Policy',
            'Set Discount Policy', 'Get Discount Policy', 'Get Store Purchase History'].map((value) => {
                const labelId = `checkbox-list-label-${value}`;

                return (
                <ListItem key={value} role={undefined} dense button onClick={handleToggle(value)}>
                    <ListItemIcon>
                    <Checkbox
                        edge="start"
                        checked={checked.indexOf(value) !== -1}
                        tabIndex={-1}
                        disableRipple
                        inputProps={{ 'aria-labelledby': labelId }}
                        onChange={(e, isInputChecked) => handleCheck(e, isInputChecked, value)}
                    />
                    </ListItemIcon>
                    <ListItemText id={labelId} primary={`${value}`} />
                    <ListItemSecondaryAction>
                    {/* <IconButton edge="end" aria-label="comments">
                        <CommentIcon />
                    </IconButton> */}
                    </ListItemSecondaryAction>
                </ListItem>
                );
            })}
        </List>
    )
}

export default CheckboxList;
