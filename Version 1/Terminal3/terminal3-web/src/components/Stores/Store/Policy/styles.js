import { makeStyles } from '@material-ui/core/styles';

export default makeStyles((theme) => ({
    root: {
        height: 110,
        flexGrow: 1,
        maxWidth: 400,
      },
      labelText: {
          fontWeight: 'inherit',
          flexGrow: 1,
          'padding-right': theme.spacing(2),
      },
      labelRoot: {
          display: 'flex',
          alignItems: 'center',
          padding: theme.spacing(0.5, 0),
      },
      paper: {
          marginTop: theme.spacing(8),
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
      },
      avatar: {
          margin: theme.spacing(1),
          backgroundColor: theme.palette.secondary.main,
      },
    }));