import React from 'react'

const Cars = React.lazy(() => import('./app/main/cars/Cars'))
const AddCar = React.lazy(() => import('./app/main/cars/AddEditCar'))

const routes = [
  //Car
  { path: '/cars', name: 'cars', element: Cars },
  { path: '/cars/add-car', name: 'AddCar', element: AddCar },
  { path: '/cars/edit-car/:ModuleId', name: 'EditCar', element: AddCar },
]

export default routes
