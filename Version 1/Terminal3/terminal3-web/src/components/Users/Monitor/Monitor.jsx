import React, { useState } from 'react'
import { Avatar, Button, CssBaseline, TextField, FormControlLabel, Checkbox, Link, Grid, Box, Typography, Container } from '@material-ui/core';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import { useHistory } from 'react-router-dom';
import {Action} from '../../../components';
import {LineChart} from '../../../components'
import  {createDataGroup} from '../../Charts/LineChart'
import {GetSystemMonitorRecords,printErrorMessage} from '../../../api/API'
import useStyles from './styles';

function Copyright() {
    return (
      <Typography variant="body2" color="textSecondary" align="center">
        {'Copyright © '}
        <Link color="inherit" href="https://material-ui.com/">
          Terminal 3
        </Link>{' '}
        {new Date().getFullYear()}
        {'.'}
      </Typography>
    );
}


const Monitor = ({ userID }) => {
    // styles.js
    const classes = useStyles();
  
    // states
    const [data, setData] = useState({});

    const sample = createDataGroup('this is first name',['1','2','3','4','5','6','7'],[1,2,3,4,5,6,7]);
    const sample1 = createDataGroup('this is second name',['1','2','3','4','5','6','7'],[1,2,3,3,3,6,7]);
    //console.log(sample);

    const handleGetSystemMonitorRecords = (data) => {
        console.log(data)
        const toSend = {startDate: data.startdate, endDate: data.enddate, AdminID: userID};

        GetSystemMonitorRecords(toSend)
            .then(response => response.ok ?
                response.json().then(result => console.log(result) ) : printErrorMessage(response)).catch(err => console.log(err));
    }

    return (
    <Container component="main" maxWidth="xs">
        <CssBaseline />
            <div className={classes.paper}>
                <Avatar className={classes.avatar}> <LockOutlinedIcon /> </Avatar>
                <Typography component="h1" variant="h5"> Monitor System </Typography>
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        color="primary"
                        className={classes.submit}
                    >
                        Start live monitor
                    </Button>
                    <Action name='System Status by Day'
                            fields={[{name: 'Start Date', required: true, type: 'date'},
                                    {name: 'End Date', required: true, type: 'date'}]}   
                            handleAction={handleGetSystemMonitorRecords}/>
                    <Grid container spacing={2}>
                        < LineChart DataGroups={[sample,sample1]} />
                    </Grid>
            </div>
        <Box mt={5}>
            <Copyright />
        </Box>
    </Container>
    );
}

export default Monitor;
