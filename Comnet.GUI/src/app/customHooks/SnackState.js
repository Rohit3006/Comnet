import { useState } from 'react'

export default function SnackState() {
  const [snack, setSnack] = useState({
    openSnack: false,
    message: '',
    severity: '',
  })
  const closeSnack = (event, reason) => {
    if (reason === 'clickaway') {
      return
    }
    setSnack((prevState) => ({ ...prevState, openSnack: false }))
  }

  const showSnackbar = (message, severity = '') => {
    setSnack({ openSnack: true, message, severity })
  }

  return [snack, closeSnack, showSnackbar]
}
