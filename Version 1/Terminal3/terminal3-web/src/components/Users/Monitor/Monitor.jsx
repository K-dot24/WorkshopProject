import React, { useState,useEffect } from 'react'
import { Avatar, Button, CssBaseline, TextField, FormControlLabel, Checkbox, Link, Grid, Box, Typography, Container } from '@material-ui/core';
import LiveTvIcon from '@material-ui/icons/LiveTv';
import { useHistory } from 'react-router-dom';
import {Action} from '../../../components';
import {LineChart,BarChart} from '../../../components'
import  {createDataGroup} from '../../Charts/LineChart'
import  {createLiveSample} from '../../Charts/BarChart'
import {GetSystemMonitorRecords,printErrorMessage,StartMonitorRequest} from '../../../api/API'
import useStyles from './styles';
import { HubConnectionBuilder } from '@microsoft/signalr';

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


function prepareData(data){
    let dates=[];
    let admins=[];
    let guestUsers=[];
    let managersNotOwners=[];
    let owners=[];
    let registeredUsers=[];

    data.map((record)=>{
        dates.push(record.date);
        admins.push(record.admins);
        guestUsers.push(record.guestUsers);
        managersNotOwners.push(record.managersNotOwners);
        owners.push(record.owners);
        registeredUsers.push(record.registeredUsers);

    })
    let adminsData = createDataGroup('System Admins',dates,admins);
    let guestsData = createDataGroup('Guest Users',dates,guestUsers);
    let managersData = createDataGroup('Managers',dates,managersNotOwners);
    let ownersData = createDataGroup('Owners',dates,owners);
    let registerData = createDataGroup('Register Users',dates,registeredUsers);
    return [registerData,guestsData,ownersData,managersData,adminsData] ;


}

const Monitor = ({ userID }) => {
    // styles.js
    const classes = useStyles();

    // states
    const [data, setData] = useState([]);
    const [livesample,setLiveSample] = useState([])

    // SignalR
    const [connection, setConnection] = useState(null);

    const handleGetSystemMonitorRecords = (data) => {
        console.log(data)
        const toSend = {startDate: data.startdate, endDate: data.enddate, AdminID: userID};

        GetSystemMonitorRecords(toSend)
            .then(response => response.ok ?
                response.json().then(result => setData(prepareData(result.data)) ) : printErrorMessage(response)).catch(err => console.log(err));
    }

    useEffect(() => {
        // SignalR - Create new connection
        console.log('Creating new connection');
        var newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:4001/signalr/notification')
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
        console.log('after connection');
    }, []);

    // SignalR
    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Streaming socket connected!');
                    connection.on('ReceiveMonitor', sample => {
                        setLiveSample(sample);
                        console.log(sample);
                    }); 

                    //requesting first sample from the system
                   /* StartMonitorRequest(userID)
                    .then(response => response.ok ?
                        response.json().then(result => console.log(result) ) : printErrorMessage(response)).catch(err => console.log(err));*/

                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    return (
    <Container component="main" maxWidth="xs">
        <CssBaseline />
            <div className={classes.paper}>
                <Avatar className={classes.avatar}> <LiveTvIcon /> </Avatar>
                <Typography component="h1" variant="h5"> Live System Monitor </Typography>
                <BarChart  sample={livesample}/>
                    <Action name='System Status by Day'
                            fields={[{name: 'Start Date', required: true, type: 'date'},
                                    {name: 'End Date', required: true, type: 'date'}]}   
                            handleAction={handleGetSystemMonitorRecords}/>
                        < LineChart DataGroups={data} />
            </div>
        <Box mt={5}>
            <Copyright />
        </Box>
    </Container>
    );
}

export default Monitor;
