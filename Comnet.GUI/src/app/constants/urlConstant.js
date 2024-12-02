const API_URL = import.meta.env.VITE_API_URL

const urlConstant = {
  car: {
    carAdd: `${API_URL}Car/AddCar`,
    carEdit: `${API_URL}Car/UpdateCar`,
    carDelete: `${API_URL}Car/DeleteCar`,
    carList: `${API_URL}Car/GetCarList`,
    carById: `${API_URL}Car/GetCarById`,
    carDropdown: `${API_URL}Car/GetCarDropDown`,
  },
}

export default urlConstant
