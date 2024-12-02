import React, { Suspense } from 'react'
import { Navigate, Route, Routes } from 'react-router-dom'
import { CContainer, CSpinner } from '@coreui/react'

// routes config
import routes from '../routes'
import { Backdrop, Box, CircularProgress } from '@mui/material'

const AppContent = () => {
  return (
    <CContainer className="px-4" lg>
      <Suspense
        fallback={
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
        }
      >
        <Routes>
          {routes.map((route, idx) => {
            return (
              route.element && (
                <Route
                  key={idx}
                  path={route.path}
                  exact={route.exact}
                  name={route.name}
                  // element={<AuthenticatedRoute element={route.element} />}
                  element={<route.element />}
                />
              )
            )
          })}
          <Route path="/" element={<Navigate to={`cars`} replace />} />
          {/* <Route path="/" element={<></>} /> */}
          {/* <Route path="*" element={<Navigate to="404" replace />} /> */}
        </Routes>
      </Suspense>
    </CContainer>
  )
}

export default React.memo(AppContent)
