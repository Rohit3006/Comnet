const dataConstant = {
  apiStatus: {
    success: 'Success',
    OK: 'OK',
    failed: 'Failure',
    exists: 'Exists',
    notFound: 'NotFound',
    empty: 'Empty',
  },
  tableRequest: {
    pageNumber: 1,
    rowsPerPage: 10,
    searchText: '',
    sortColumns: {
      sortColumnName: '',
      sortOrder: '',
    },
  },
  tableUnqGUIDRequest: {
    pageNumber: 1,
    rowsPerPage: 10,
    searchText: '',
    sortColumns: {
      sortColumnName: '',
      sortOrder: '',
    },
  },
  pagination: {
    pageNumber: 1,
    defaultPageSize: 10,
    pageSizeOptions: [10, 25, 50, 100],
  },
  backdropClick: 'backdropClick',

  brand: ['Audi', 'Jaguar', 'Land rover', 'Renault'],
  class: ['A-Class', 'B-Class', 'C-Class'],
}

export default dataConstant
