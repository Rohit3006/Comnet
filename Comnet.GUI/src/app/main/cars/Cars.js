import { Button, IconButton, Paper, Typography } from '@mui/material'
import urlConstant from '../../constants/urlConstant'
import DataGridCustom from '../../shared-components/DataGrid/DataGrid'
import { Add, Delete, Edit } from '@mui/icons-material'
import { useState } from 'react'
import DialogCustom from '../../shared-components/Dialog/DialogCustom'
import axiosAPI from '../../utils/axiosAPI'
import dataConstant from '../../constants/dataConstant'
import SnackBarCustom from '../../shared-components/Snackbar/SnackBarCustom'
import SnackState from '../../customHooks/SnackState'
import { NavLink } from 'react-router-dom'
import { trackPromise } from 'react-promise-tracker'

const DOMIN_URL = import.meta.env.VITE_DOMIN_URL

const Cars = () => {
  const [modal, setModal] = useState()
  const [snack, closeSnack, showSnackbar] = SnackState()

  const columns = [
    {
      field: 'url',
      headerName: 'Image',
      width: 150,
      renderCell: (params) => {
        return params.value ? <img src={`${DOMIN_URL}${params.value}`} width={100} /> : <></>
      }, // renderCell will render the component
    },
    {
      field: 'name',
      headerName: 'Name',
      flex: 1,
    },
    {
      field: 'code',
      headerName: 'Code',
      flex: 1,
    },
    {
      field: 'actions',
      headerName: 'Actions',
      width: 150,
      flex: 0,
      sortable: false,
      renderCell: (params) => (
        <>
          {/* Edit Icon/Button */}
          <NavLink to={`edit-car/${params.row.unqGUID}`} color="primary">
            <Edit />
          </NavLink>
          {/* Delete Icon/Button */}
          <IconButton
            color="secondary"
            onClick={() => {
              setModal({
                unqGUID: params.row.unqGUID,
                open: true,
                title: 'Delete car',
                descritpion: 'Do you want to delete the car record?',
                yes: 'Yes',
                no: 'No',
              })
            }}
          >
            <Delete />
          </IconButton>
        </>
      ),
    },
  ]

  const deleteRecord = () => {
    trackPromise(
      axiosAPI
        .delete(urlConstant.car.carDelete, {
          data: {
            guid: modal.unqGUID,
          },
        })
        .then((response) => {
          if (response.status === dataConstant.apiStatus.success) {
            showSnackbar('Record deleted!', 'error')
          } else if (response.status === dataConstant.apiStatus.failed) {
            showSnackbar('Something went wrong', 'error')
          }
          setModal(null)
        }),
    )
  }

  return (
    <Paper style={{ padding: '20px' }}>
      <Typography variant="h6">Cars</Typography>

      <DataGridCustom
        url={urlConstant.car.carList}
        columns={columns}
        buttonChildren={
          <NavLink to={`add-car`} color="primary">
            <Button variant="contained" color="primary" startIcon={<Add />}>
              Add Car
            </Button>
          </NavLink>
        }
      />

      <DialogCustom
        open={modal?.open ?? false}
        title={modal?.title ?? ''}
        details={modal?.descritpion ?? ''}
        closeModal={() => setModal(null)}
        popUpButtons={
          <>
            <Button color="success" onClick={() => setModal(null)}>
              {modal?.no ?? ''}
            </Button>
            <Button
              variant="contained"
              color="error"
              onClick={() => {
                deleteRecord()
              }}
            >
              {modal?.yes ?? ''}
            </Button>
          </>
        }
      />

      <SnackBarCustom snackObj={snack} closeSnack={closeSnack} buttonProp duration={2000} />
    </Paper>
  )
}

export default Cars
