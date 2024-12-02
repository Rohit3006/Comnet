import React, { useEffect, useState } from 'react'
import { usePromiseTracker } from 'react-promise-tracker'
import { Box, Backdrop, CircularProgress } from '@mui/material'

const Spinner = () => {
  const { promiseInProgress } = usePromiseTracker()
  const [showSpinner, setShowSpinner] = useState(false)

  useEffect(() => {
    let timeout

    if (promiseInProgress) {
      // Show the spinner immediately
      setShowSpinner(true)
    } else {
      // If the promise is resolved, set a timeout to hide the spinner after 0.5 seconds
      // timeout = setTimeout(() => {
      setShowSpinner(false)
      // }, 500);
    }

    return () => clearTimeout(timeout)
  }, [promiseInProgress])

  return (
    showSpinner && (
      <>
        <Backdrop open sx={{ backgroundColor: 'rgba(252, 253, 247, 0.5)' }}>
          <Box
            sx={{
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
            }}
          >
            <CircularProgress color="primary" />
          </Box>
        </Backdrop>
      </>
    )
  )
}
export default Spinner
