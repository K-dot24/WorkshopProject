import { React, useState, useEffect } from 'react';
import { Container, CssBaseline } from '@material-ui/core';
import { TreeView, TreeItem } from '@material-ui/lab';
import { ExpandMore, ChevronRight } from '@material-ui/icons';
import { makeStyles } from '@material-ui/core/styles';

import { GetDiscountPolicyData, printErrorMessage } from '../../../../api/API';

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

//#region Styles
const useStyles = makeStyles((theme) => ({
    root: {
      height: 110,
      flexGrow: 1,
      maxWidth: 400,
    },
    toolbar: theme.mixins.toolbar,
    layout: {
        marginTop: '5%',
        width: 'auto',
        marginLeft: theme.spacing(2),
        marginRight: theme.spacing(2),
        [theme.breakpoints.up(600 + theme.spacing(2) * 2)]: {
            width: 600,
            marginLeft: 'auto',
            marginRight: 'auto',
        },
    },
}));
//#endregion

const Policy = ({ storeID }) => {
    const [data, setData] = useState(null);
    const classes = useStyles();

    // API Calls

    const handleGetDiscountPolicyData = () => {
        GetDiscountPolicyData(storeID).then(response => response.ok ? 
            response.json().then(result => setData(result.data)) : printErrorMessage(response)).catch(err => alert(err));
    }

    const renderTree = (nodes) => (
        <TreeItem key={nodes.id} nodeId={nodes.id} label={nodes.name}>
            {Array.isArray(nodes.children) ? nodes.children.map((node) => renderTree(node)) : null}
        </TreeItem>
    );

    useEffect(() => {
        console.log("Fetching Data");
        handleGetDiscountPolicyData();
    }, [])

    useEffect(() => {
        console.log(data);
    }, [data])

    return (
    <>
        <div className={classes.toolbar} />
        <main className={classes.layout}>
            <Container component="main" maxWidth="xs">
                <CssBaseline />
                <TreeView
                    className={classes.root}
                    defaultCollapseIcon={<ExpandMore />}
                    defaultExpanded={['root']}
                    defaultExpandIcon={<ChevronRight />}
                >
                    {renderTree(mockData)}
                </TreeView>
            </Container>
        </main> 
    </>
    );
}

export default Policy;
