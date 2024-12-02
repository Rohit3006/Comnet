import axios from 'axios'

const axiosAPI = axios.create({
  // baseURL: DataConstant.API_URL,
  timeout: 240000,
  headers: { 'Content-Type': 'application/json' },
})

// axiosAPI.interceptors.request.use(
//   async (config) => {
//     var user = JSON.parse(localStorage?.getItem('user') ?? '')
//     const token = user?.token ?? ''
//     config.headers.Authorization = `Bearer ${token}`
//     return config
//   },
//   (error) => {
//     return Promise.reject(error)
//   },
// )

axiosAPI.interceptors.response.use(
  (response) => {
    return response?.data ? response?.data : response
  },
  (error) => {
    // if (error?.response?.status === 401) {
    // window.location.href = '/#/login' // Redirect to the login page
    // }
    return Promise.reject(error)
  },
)

export default axiosAPI
