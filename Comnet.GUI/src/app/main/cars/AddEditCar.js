import {
  Autocomplete,
  Button,
  Card,
  CardActions,
  CardMedia,
  Checkbox,
  FormControlLabel,
  FormHelperText,
  Grid,
  IconButton,
  Paper,
  TextField,
  Typography,
} from '@mui/material'
import SnackState from '../../customHooks/SnackState'
import { NavLink, useNavigate, useParams } from 'react-router-dom'
import { Controller, useForm } from 'react-hook-form'
import { useEffect } from 'react'
import { trackPromise } from 'react-promise-tracker'
import SnackBarCustom from '../../shared-components/Snackbar/SnackBarCustom'
import urlConstant from '../../constants/urlConstant'
import axiosAPI from '../../utils/axiosAPI'
import dataConstant from '../../constants/dataConstant'
import { DatePicker, DateTimePicker, LocalizationProvider } from '@mui/x-date-pickers'
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import dayjs from 'dayjs'
import ReactQuill from 'react-quill'
import 'react-quill/dist/quill.snow.css'
import { Delete, UploadFile } from '@mui/icons-material'
import axios from 'axios'

const MAX_FILE_SIZE = 5 * 1024 * 1024 // 5MB in bytes
const DOMIN_URL = import.meta.env.VITE_DOMIN_URL

const AddCar = () => {
  const [snack, closeSnack, showSnackbar] = SnackState()
  const navigate = useNavigate()
  const { ModuleId } = useParams()
  const { control, handleSubmit, setValue, watch } = useForm({
    defaultValues: {
      name: '',
      code: '',
      brand: '',
      class: '',
      price: '',
      manufacturingDate: dayjs(new Date()),
      isActive: true,
      sortOrder: '',
      description: '',
      features: '',
      files: [],
    },
  })
  const selectedFiles = watch('files', [])

  const handleFileChange = (event) => {
    const files = Array.from(event.target.files)
    const validFiles = files.filter((file) => file.size <= MAX_FILE_SIZE)

    if (validFiles.length === 0) {
      alert('No valid files selected (should be under 5MB).')
      return
    }

    const updatedFiles = validFiles.map((file) => {
      const reader = new FileReader()
      return new Promise((resolve) => {
        reader.onloadend = () => {
          resolve({
            file,
            preview: reader.result,
          })
        }
        reader.readAsDataURL(file)
      })
    })

    Promise.all(updatedFiles).then((fileData) => {
      // Update the form value
      setValue('files', [...selectedFiles, ...fileData])
    })
  }
  const removeFile = (index) => {
    if (selectedFiles[index].unqGUID) {
      selectedFiles[index].isDeleted = true
      setValue('files', selectedFiles)
    } else {
      setValue(
        'files',
        selectedFiles.filter((_, i) => i !== index),
      )
    }
  }
  useEffect(() => {
    if (ModuleId) {
      getDetailsById()
    }
  }, [ModuleId])

  const onSubmit = (data) => {
    const requestURL = ModuleId ? urlConstant.car.carEdit : urlConstant.car.carAdd

    const formData = new FormData()

    // Append files to FormData
    data.files?.forEach((item, index) => {
      item.unqGUID && formData.append(`files[${index}].UnqGUID`, item.unqGUID)
      item.isDeleted && formData.append(`files[${index}].IsDeleted`, item.isDeleted)
      item.file && formData.append(`files[${index}].ImageFile`, item.file)
      formData.append(`files[${index}].ImageOrder`, index)
    })

    // Dynamically append other fields to FormData
    for (const key in data) {
      if (key !== 'files') {
        // Skip files since they are already appended
        formData.append(key, data[key])
      }
    }

    trackPromise(
      axios
        .post(requestURL, formData, {
          headers: {
            'Content-Type': 'multipart/form-data',
          },
        })
        .then((response) => {
          if (
            response.data.status?.toLowerCase() === dataConstant.apiStatus.success.toLowerCase()
          ) {
            showSnackbar(ModuleId ? `Record updated` : `Record added`, 'success')
            setTimeout(() => {
              navigate(`/cars`)
            }, 1000)
          } else if (response.data.status === dataConstant.apiStatus.exists)
            showSnackbar(`Record exists`, 'info')
          else {
            showSnackbar(`Something went wrong`, 'error')
          }
        }),
    )
  }

  const getDetailsById = () => {
    trackPromise(
      axiosAPI.get(`${urlConstant.car.carById}?id=${ModuleId}`).then((response) => {
        if (response) {
          if (response.status?.toLowerCase() === dataConstant.apiStatus.success.toLowerCase()) {
            const responseData = response.data
            for (const key in responseData) {
              if (key === 'manufacturingDate') {
                setValue(key, dayjs(responseData[key]))
              } else if (responseData[key] != null) {
                setValue(key, responseData[key])
              }
            }
          } else {
            showSnackbar(`Something went wrong`, 'error')
          }
        }
      }),
    )
  }

  return (
    <form name="userForm" autoComplete="off" onSubmit={handleSubmit(onSubmit)}>
      <Paper style={{ padding: '20px' }}>
        <Typography variant="h6">Car</Typography>
        <Grid container spacing={2}>
          <Grid item xs={12} sm={6}>
            <Controller
              name="brand"
              control={control}
              rules={{
                required: 'Brand is required',
              }}
              render={({ field: { ref, value, onChange, ...field }, fieldState }) => {
                return (
                  <Autocomplete
                    {...field}
                    value={value}
                    onChange={(e, newValue) => {
                      onChange(newValue)
                    }}
                    ListboxProps={{
                      style: { maxHeight: 200, overflow: 'auto' },
                    }}
                    // getOptionLabel={(option) => option.name}
                    options={dataConstant.brand}
                    filterSelectedOptions
                    // noOptionsText={t('exceptionModule.noOptionsMessage')}
                    isOptionEqualToValue={(options, value) => options === value}
                    renderInput={(params) => (
                      <TextField
                        {...params}
                        inputRef={ref}
                        label={'Brand'}
                        className="mui-form-text-field"
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}
                      />
                    )}
                  />
                )
              }}
            />
            <Controller
              name="class"
              control={control}
              rules={{
                required: 'Class is required',
              }}
              render={({ field: { ref, value, onChange, ...field }, fieldState }) => {
                return (
                  <Autocomplete
                    {...field}
                    value={value}
                    onChange={(e, newValue) => {
                      onChange(newValue)
                    }}
                    ListboxProps={{
                      style: { maxHeight: 200, overflow: 'auto' },
                    }}
                    getOptionLabel={(option) => option}
                    options={dataConstant.class}
                    filterSelectedOptions
                    // noOptionsText={t('exceptionModule.noOptionsMessage')}
                    isOptionEqualToValue={(options, value) => options === value}
                    renderInput={(params) => (
                      <TextField
                        {...params}
                        inputRef={ref}
                        label={'Class'}
                        className="mui-form-text-field"
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}
                      />
                    )}
                  />
                )
              }}
            />
            <Controller
              name="name"
              control={control}
              rules={{
                required: `Name is required`,
                maxLength: {
                  value: 100,
                  message: `Max length is 100 characters`,
                },
                validate: (value) => {
                  if (value.trim() === '') {
                    return `Name is required`
                  }
                  return true
                },
              }}
              render={({ field: { ref, value, ...field }, fieldState }) => {
                return (
                  <TextField
                    {...field}
                    inputRef={ref}
                    size="medium"
                    type="text"
                    className="mui-form-text-field"
                    label="Model name"
                    variant="outlined"
                    value={value}
                    error={!!fieldState.error}
                    helperText={fieldState.error?.message}
                  />
                )
              }}
            />
            <Controller
              name="code"
              control={control}
              rules={{
                required: `Code is required`,
                maxLength: {
                  value: 4000,
                  message: `Max length is 4000 characters`,
                },
                validate: (value) => {
                  if (value.trim() === '') {
                    return `Code is required`
                  }
                  return true
                },
              }}
              render={({ field: { ref, value, ...field }, fieldState }) => {
                return (
                  <TextField
                    {...field}
                    inputRef={ref}
                    size="medium"
                    type="text"
                    label="Model code"
                    className="mui-form-text-field"
                    variant="outlined"
                    value={value}
                    error={!!fieldState.error}
                    helperText={fieldState.error?.message}
                  />
                )
              }}
            />
            <Controller
              name="price"
              control={control}
              rules={{
                required: `Price is required`,
                min: {
                  value: 0,
                  message: 'Invalid number',
                },
              }}
              render={({ field: { ref, value, ...field }, fieldState }) => {
                return (
                  <TextField
                    {...field}
                    inputRef={ref}
                    size="medium"
                    type="number"
                    label="Price"
                    className="mui-form-text-field"
                    variant="outlined"
                    value={value}
                    error={!!fieldState.error}
                    helperText={fieldState.error?.message}
                  />
                )
              }}
            />
            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <Controller
                name="manufacturingDate"
                control={control}
                // defaultValue={new Date()}
                rules={{
                  required: 'Manufacturing date is required',
                }}
                render={({ field: { ref, value, onChange, ...field }, fieldState }) => (
                  <DateTimePicker
                    // key={Math.random()}
                    {...field}
                    ref={ref}
                    value={value ?? new Date()}
                    onChange={(val) => {
                      if (val) onChange(val)
                    }}
                    label={'Manufacturing date'}
                    className="mui-form-text-field"
                    // inputFormat={userDetails?.dateFormat}
                    renderInput={(params) => (
                      <TextField
                        {...params}
                        className="mui-form-text-field"
                        error={!!fieldState.error}
                        helperText={fieldState.error?.message}
                      />
                    )}
                  />
                )}
              />
            </LocalizationProvider>
            <Controller
              name="isActive"
              control={control}
              render={({ field: { ref, value, ...field } }) => (
                <FormControlLabel
                  className="mui-form-checkbox"
                  label="Is Active"
                  {...field}
                  value={value}
                  control={<Checkbox checked={value} />}
                />
              )}
            />
            <Controller
              name="sortOrder"
              control={control}
              rules={{
                min: {
                  value: 0,
                  message: 'Invalid number',
                },
              }}
              render={({ field: { ref, value, ...field }, fieldState }) => {
                return (
                  <TextField
                    {...field}
                    inputRef={ref}
                    size="medium"
                    type="number"
                    label="Sort order"
                    className="mui-form-text-field"
                    variant="outlined"
                    value={value}
                    error={!!fieldState.error}
                    helperText={fieldState.error?.message}
                  />
                )
              }}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <Controller
              name="description"
              control={control}
              rules={{
                required: 'Description is required',
              }}
              render={({ field: { ref, value, onChange, ...field }, fieldState }) => {
                return (
                  <>
                    <ReactQuill
                      {...field}
                      value={value}
                      onChange={(val) => onChange(val)}
                      theme="snow"
                      placeholder="Description"
                      className="mui-form-text-field"
                    />

                    <FormHelperText error={!!fieldState.error}>
                      {fieldState.error?.message}
                    </FormHelperText>
                  </>
                )
              }}
            />
            <Controller
              name="features"
              control={control}
              rules={{
                required: 'Features is required',
              }}
              render={({ field: { ref, value, onChange, ...field }, fieldState }) => {
                return (
                  <>
                    <ReactQuill
                      {...field}
                      value={value}
                      onChange={(val) => onChange(val)}
                      theme="snow"
                      placeholder="Features"
                      className="mui-form-text-field"
                    />

                    <FormHelperText error={fieldState.error}>
                      {fieldState.error?.message}
                    </FormHelperText>
                  </>
                )
              }}
            />
            <Controller
              name="files"
              control={control}
              render={({ field: { ref, value, onChange, ...field } }) => (
                <>
                  <Button
                    {...field}
                    variant="contained"
                    component="label"
                    startIcon={<UploadFile />}
                    sx={{ width: '100%', height: 100 }}
                  >
                    Select Images
                    <input
                      type="file"
                      accept="image/*"
                      multiple
                      hidden
                      onChange={handleFileChange}
                    />
                  </Button>
                  <Grid container spacing={2}>
                    {selectedFiles.map((item, index) =>
                      !item?.isDeleted ? (
                        <Grid
                          item
                          xs={12}
                          sm={6}
                          md={4}
                          key={index}
                          className="mui-form-text-field"
                        >
                          <Card>
                            <CardMedia
                              component="img"
                              height="150"
                              image={item.preview ? item.preview : `${DOMIN_URL}${item.imagesUrl}`}
                              alt={`Preview ${index}`}
                            />
                            <CardActions>
                              <IconButton
                                color="error"
                                onClick={() => removeFile(index)}
                                aria-label="delete"
                              >
                                <Delete />
                              </IconButton>
                            </CardActions>
                          </Card>
                        </Grid>
                      ) : null,
                    )}
                  </Grid>
                </>
              )}
            />
          </Grid>
        </Grid>
      </Paper>
      <div className="save-cancel-container">
        <NavLink to={`/car`} color="primary">
          <Button variant="contained" color="secondary">
            Cancel
          </Button>
        </NavLink>
        <Button type="submit" variant="contained" color="primary">
          Save
        </Button>
      </div>
      <SnackBarCustom snackObj={snack} closeSnack={closeSnack} buttonProp />
    </form>
  )
}

export default AddCar
