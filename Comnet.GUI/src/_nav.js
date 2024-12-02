import React from 'react'
import { CNavItem } from '@coreui/react'
import { DirectionsCarFilled } from '@mui/icons-material'

const _nav = [
  {
    component: CNavItem,
    name: 'Cars',
    to: '/cars',
    icon: <DirectionsCarFilled sx={{ mr: 1 }} />,
  },
]

export default _nav
