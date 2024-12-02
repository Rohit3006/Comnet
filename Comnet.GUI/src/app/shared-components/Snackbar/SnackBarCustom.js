import MuiAlert from '@mui/material/Alert'
import Snackbar from '@mui/material/Snackbar'

const Alert = (props) => {
  return <MuiAlert elevation={6} variant="filled" {...props} />
}

function SnackBarCustom({ snackObj, closeSnack, buttonProp, duration }) {
  const { openSnack, message, severity } = snackObj
  return (
    <>
      <Snackbar open={openSnack} autoHideDuration={duration || 2000} onClose={closeSnack} anchorOrigin={{ vertical: 'top', horizontal: 'right' }}>
        <div>
          <Alert onClose={closeSnack} severity={severity}>
            <div>
              {message}
              {buttonProp}
            </div>
          </Alert>
        </div>
      </Snackbar>
    </>
  )
}

export default SnackBarCustom
