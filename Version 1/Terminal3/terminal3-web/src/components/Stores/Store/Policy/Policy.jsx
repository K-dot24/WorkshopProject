import { React, useState, useEffect } from 'react';
import { Container, CssBaseline, Typography, Avatar } from '@material-ui/core';
import { TreeView, TreeItem } from '@material-ui/lab';
import { ExpandMore, ChevronRight, Receipt } from '@material-ui/icons';

import useStyles from './styles';
import { GetDiscountPolicyData, GetPurchasePolicyData, printErrorMessage } from '../../../../api/API';

const mockData = {
    id: 'root',
    name: 'Parent',
    children: [
      {
        id: '1',
        name: 'Child - 1',
      },
      {
        id: '3',
        name: 'Child - 3',
        children: [
          {
            id: '4',
            name: 'Child - 4',
          },
        ],
      },
    ],
};

const Policy = ({ match, storeID }) => {
    const [data, setData] = useState(null);
    const classes = useStyles();

    //#region API Calls

    const handleGetDiscountPolicyData = () => {
        GetDiscountPolicyData(storeID).then(response => response.ok ? 
            response.json().then(result => setData(result.data)) : printErrorMessage(response)).catch(err => alert(err));
    }

    const handleGetPurchasePolicyData = () => {
        GetPurchasePolicyData(storeID).then(response => response.ok ? 
            response.json().then(result => setData(result.data)) : printErrorMessage(response)).catch(err => alert(err));
    }

    //#endregion

    const renderTree = (nodes) => (
        <TreeItem key={nodes.id} nodeId={nodes.id}
                label={
                    <div className={classes.labelRoot}>
                      <Typography variant="body2" className={classes.labelText}>
                        {nodes.name}
                      </Typography>
                      <Typography variant="caption" color="inherit">
                        {nodes.id}
                      </Typography>
                    </div>
                  }
        >
            {Array.isArray(nodes.children) ? nodes.children.map((node) => renderTree(node)) : null}
        </TreeItem>
    );

    useEffect(() => {
        if (match.url.includes('getdiscountpolicy'))
            handleGetDiscountPolicyData();
        else if (match.url.includes('getpurchasepolicy'))
            handleGetPurchasePolicyData();
    }, [match])

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline />
            <div className={classes.paper}>
                <Avatar className={classes.avatar}>
                    <Receipt />
                </Avatar>
                <Typography component="h1" variant="h5">
                    {match.url.includes('getdiscountpolicy') ? "Discount Policy Tree" : "Purchase Policy Tree"}
                </Typography>
                <br/>
                <TreeView
                    className={classes.root}
                    defaultCollapseIcon={<ExpandMore />}
                    // defaultExpanded={['root']}
                    defaultExpandIcon={<ChevronRight />}
                >
                    {data !== null && renderTree(data)}
                </TreeView>
            </div>
        </Container>
    );
}

export default Policy;
