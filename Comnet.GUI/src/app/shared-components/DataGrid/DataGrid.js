import { DataGrid } from '@mui/x-data-grid'
import dataConstant from '../../constants/dataConstant'
import { useEffect, useState } from 'react'
import axiosAPI from '../../utils/axiosAPI'
import { Box, TextField } from '@mui/material'
import useDebounce from '../../customHooks/useDebounce'
import { trackPromise } from 'react-promise-tracker'

const DataGridCustom = ({ url, columns, pageSizeOptions, buttonChildren, tableRequest }) => {
  const [isAPICalled, setIsAPICalled] = useState(false)
  const [searchText, setSearchText] = useState('')
  const [tableRequestParam, setTableRequestParam] = useState(
    JSON.parse(JSON.stringify(tableRequest ?? dataConstant.tableRequest)),
  )
  const [dataList, setDataList] = useState({
    count: 0,
    data: [],
  })

  useEffect(() => {
    getDataList(tableRequestParam)
  }, [tableRequestParam])

  const debouncedSearch = useDebounce(searchText, 1000)
  useEffect(() => {
    setTableRequestParam({
      ...tableRequestParam,
      pageNumber: 1,
      searchText: debouncedSearch,
    })
  }, [debouncedSearch])

  const getDataList = (requestObject) => {
    setIsAPICalled(false)
    trackPromise(
      axiosAPI.post(url, requestObject).then((response) => {
        setDataList({
          data: response.data.list,
          count: response.data.totalCount,
        })
      }),
    )
    setIsAPICalled(true)
  }

  return isAPICalled ? (
    <>
      <Box sx={{ marginTop: 2, marginBottom: 2 }}>
        <div className="filter-wrapper">
          <div className="search-bar">
            <TextField
              size="small"
              name="searchtext"
              type="search"
              id="data"
              label="Search"
              variant="outlined"
              onChange={(e) => setSearchText(e.target.value)}
            />
          </div>
          {buttonChildren}
        </div>
      </Box>
      <DataGrid
        rows={dataList.data}
        columns={columns}
        rowCount={dataList.count}
        sortingMode="server"
        paginationModel={{
          page: tableRequestParam.pageNumber - 1,
          pageSize: tableRequestParam.rowsPerPage ?? dataConstant.pagination.defaultPageSize,
        }}
        sortModel={[
          {
            field: tableRequestParam.sortColumns.sortColumnName ?? '',
            sort: tableRequestParam.sortColumns.sortOrder ?? 'asc',
          },
        ]}
        paginationMode="server"
        onPaginationModelChange={(pageModel) => {
          setTableRequestParam((prev) => ({
            ...prev,
            pageNumber: pageModel.page + 1,
            rowsPerPage: pageModel.pageSize,
          }))
        }}
        onSortModelChange={(sortModel) => {
          setTableRequestParam((prev) => ({
            ...prev,
            sortColumns: {
              sortColumnName: sortModel[0]?.field ?? '',
              sortOrder: sortModel[0]?.sort ?? '',
            },
          }))
        }}
        pageSizeOptions={pageSizeOptions ?? dataConstant.pagination.pageSizeOptions}
        getRowId={(row) => row.unqGUID} // Custom field as ID
        disableColumnMenu
        disableRowSelectionOnClick
        autoHeight
      />
    </>
  ) : null
}

export default DataGridCustom
