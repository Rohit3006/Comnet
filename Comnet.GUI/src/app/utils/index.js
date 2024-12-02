import { Folder, StarBorderOutlined } from '@mui/icons-material'
import { Box, Grid, Stack, Typography } from '@mui/material'
import { TreeItem } from '@mui/x-tree-view'
import dataConstant from '../constants/dataConstant'

export const removeChildNodes = (data, unqGUID) => {
  const parentIndex = data.findIndex((item) => item.unqGUID === unqGUID)
  if (parentIndex === -1) return data

  const parentId = data[parentIndex].unqGUID

  const recursivelyRemoveChildren = (data, parentId) => {
    return data.filter((item) => {
      if (item.parentID === parentId) {
        data = recursivelyRemoveChildren(data, item.unqGUID)
      }
      return item.parentID !== parentId
    })
  }

  const filteredData = recursivelyRemoveChildren(data, parentId)
  return filteredData.filter((item) => item.unqGUID !== unqGUID)
}

export const getNodesWithChildren = (data, guid) => {
  const children = []

  // Find direct children of the given GUID
  const directChildren = data.filter((item) => item.parentID === guid)

  directChildren.forEach((child) => {
    // Recursively find children of each direct child
    children.push(child)
    const nestedChildren = getNodesWithChildren(data, child.unqGUID)
    nestedChildren.forEach((item) => children.push(item))
  })

  return children
}

export const createTreeItems = (data, parentId = null) => {
  return (
    data
      ?.filter((item) => item.parentID === parentId)
      .map((item) => {
        return (
          <TreeItem
            key={item.unqGUID}
            itemId={item.unqGUID}
            label={
              <Stack direction="row">
                {item?.nodeType?.toLowerCase() === dataConstant.skill ? (
                  <StarBorderOutlined
                    sx={{ mr: 1, maxWidth: 20, display: 'flex', alignSelf: 'center' }}
                  />
                ) : item?.nodeType?.toLowerCase() === dataConstant.category ? (
                  <Folder
                    fontSize="small"
                    sx={{ mr: 1, maxWidth: 20, display: 'flex', alignSelf: 'center' }}
                  />
                ) : null}
                <Typography
                  sx={{
                    whiteSpace: 'nowrap',
                    overflow: 'hidden',
                    textOverflow: 'ellipsis',
                    width: '100%',
                    display: 'block',
                  }}
                >
                  {item.name}
                </Typography>
              </Stack>
            }
          >
            {createTreeItems(data, item.unqGUID)}
          </TreeItem>
        )
      }) ?? []
  )
}

//Not is use
// export const getGradientColor = (rating) => {
//   const value = (rating / 5) * 100

//   const r = value < 50 ? 255 : Math.floor(255 - (value - 50) * 5.1)
//   const g = value > 50 ? 255 : Math.floor(value * 5.1)
//   const b = 0
//   return `rgb(${r},${g},${b})`
// }
//Not is use

export function getColorForRating(rating) {
  const colors = [
    { value: 1.0, hex: '#ef7b7b', rgba: 'rgba(239, 123, 123, 1)' },
    { value: 1.5, hex: '#ec9f83', rgba: 'rgba(236, 159, 131, 1)' },
    { value: 2.0, hex: '#e9bd8b', rgba: 'rgba(233, 189, 139, 1)' },
    { value: 2.5, hex: '#e6d48f', rgba: 'rgba(230, 212, 143, 1)' },
    { value: 3.0, hex: '#dde396', rgba: 'rgba(221, 227, 150, 1)' },
    { value: 3.5, hex: '#c9e19d', rgba: 'rgba(201, 225, 157, 1)' },
    { value: 4.0, hex: '#bbdfa4', rgba: 'rgba(187, 223, 164, 1)' },
    { value: 4.5, hex: '#aedca7', rgba: 'rgba(174, 220, 167, 1)' },
    { value: 5.0, hex: '#aedbb5', rgba: 'rgba(174, 219, 181, 1)' },
  ]

  for (let i = 0; i < colors.length - 1; i++) {
    if (rating >= colors[i].value && rating <= colors[i + 1].value) {
      const t = (rating - colors[i].value) / (colors[i + 1].value - colors[i].value)
      const interpolate = (start, end) => Math.round(start + t * (end - start))

      const startColor = colors[i]
      const endColor = colors[i + 1]

      const r = interpolate(
        parseInt(startColor.hex.substring(1, 3), 16),
        parseInt(endColor.hex.substring(1, 3), 16),
      )
      const g = interpolate(
        parseInt(startColor.hex.substring(3, 5), 16),
        parseInt(endColor.hex.substring(3, 5), 16),
      )
      const b = interpolate(
        parseInt(startColor.hex.substring(5, 7), 16),
        parseInt(endColor.hex.substring(5, 7), 16),
      )

      const hex = `#${r.toString(16).padStart(2, '0')}${g.toString(16).padStart(2, '0')}${b.toString(16).padStart(2, '0')}`
      const rgba = `rgba(${r}, ${g}, ${b}, 1)`

      return rgba
    }
  }
  return colors[colors.length - 1]
}

export function formatTimestamp(timestamp, format) {
  if (!timestamp) {
    return '' // Return an empty string for null or undefined input
  }

  const now = new Date()
  const date = new Date(`${timestamp}${format}`)

  if (isNaN(date.getTime())) {
    return '' // Return an empty string for invalid date
  }

  const diff = now.getTime() - date.getTime() // Use getTime() for accurate milliseconds difference

  const units = [
    { label: 'day', value: 86400000 }, // 24 * 60 * 60 * 1000
    { label: 'hour', value: 3600000 }, // 60 * 60 * 1000
    { label: 'minute', value: 60000 }, // 60 * 1000
    { label: 'second', value: 1000 }, // 1000
  ]

  let timeAgo = 'Just now'

  for (const unit of units) {
    const amount = Math.floor(diff / unit.value)
    if (amount > 0) {
      timeAgo = `${amount} ${unit.label}${amount > 1 ? 's' : ''} ago`
      break // Exit loop once the most appropriate unit is found
    }
  }

  // Convert UTC date to local timezone
  const localDate = date.toLocaleDateString() // Format the date in local timezone

  return `${localDate} (${timeAgo})`
}

export function formatDate(inputDate) {
  const date = new Date(inputDate)

  // Extracting the components
  const day = String(date.getDate()).padStart(2, '0')
  const month = String(date.getMonth() + 1).padStart(2, '0') // Months are 0-based
  const year = date.getFullYear()

  return `${year}-${month}-${day}`
}

export const createHeatTreeItems = (data, parentId = null, heatMap, users, param, detailLevel) => {
  return (
    data
      ?.filter((item) => item.parentID === parentId)
      .map((item) => {
        return (
          <TreeItem
            key={item.unqGUID}
            itemId={item.unqGUID}
            label={
              <Grid container>
                <Grid item xs={6}>
                  <Stack direction="row">
                    {item?.nodeType?.toLowerCase() === dataConstant.skill ? (
                      <StarBorderOutlined
                        sx={{ mr: 1, maxWidth: 20, display: 'flex', alignSelf: 'center' }}
                      />
                    ) : item?.nodeType?.toLowerCase() === dataConstant.category ? (
                      <Folder
                        fontSize="small"
                        sx={{ mr: 1, maxWidth: 20, display: 'flex', alignSelf: 'center' }}
                      />
                    ) : null}
                    <Typography>{item.name}</Typography>
                  </Stack>
                </Grid>
                <Grid item xs={6}>
                  <Stack direction="row-reverse" spacing={0}>
                    {
                      /* {item.nodeType === dataConstant.Category && detailLevel === 1
                      ? null
                      :  */
                      users?.map((x, i) => {
                        var value
                        if (item.nodeType === dataConstant.Category) {
                          var list = childrenAverage(data, item.unqGUID, heatMap, x.userID, param)
                          value = list.length > 0 ? average(list) : 0
                        } else
                          value =
                            heatMap?.find(
                              (heat) => heat.userID === x.userID && heat.skillID === item.unqGUID,
                            )?.[param] ?? 0
                        return (
                          <Box
                            key={x.userID}
                            sx={{
                              // pt: -4,
                              mt: -0.5,
                              mb: -0.5,
                              ml: 0.3,
                              width: 30,
                              height: 30,
                              backgroundColor: value ? getColorForRating(value) : '#c7c7c7',
                              display: 'flex',
                              alignItems: 'center',
                              justifyContent: 'center',
                            }}
                          >
                            {value ? value?.toFixed(1) : null}
                          </Box>
                        )
                      })
                    }
                  </Stack>
                </Grid>
              </Grid>
            }
          >
            {createHeatTreeItems(data, item.unqGUID, heatMap, users, param, detailLevel)}
          </TreeItem>
        )
      }) ?? []
  )
}

export const childrenAverage = (data, guid, heatMap, userID, param) => {
  const children = []

  // Find direct children of the given GUID
  const directChildrenCategory = data.filter(
    (item) => item.parentID === guid && item.nodeType === dataConstant.Category,
  )
  const directChildrenSkill = heatMap.filter(
    (item) => item.categoryID === guid && item.userID === userID,
  )

  directChildrenCategory.forEach((child) => {
    // Recursively find children of each direct child
    var nestedChildren = childrenAverage(data, child.unqGUID, heatMap, userID, param)
    nestedChildren.forEach((item) => children.push(item))
  })

  directChildrenSkill.forEach((child) => {
    if (child[param]) children.push(child[param])
  })

  return children
}

export const average = (list) => list.reduce((prev, curr) => prev + curr) / list.length

export const createCapabilityTreeItems = (data, parentId = null, param, detailLevel) => {
  return (
    data
      ?.filter((item) => item.parentID === parentId)
      .map((item) => {
        return detailLevel === 2 && item.nodeType === dataConstant.Skill ? null : (
          <TreeItem
            key={item.unqGUID}
            itemId={item.unqGUID}
            label={
              <Grid container rowSpacing={2}>
                <Grid item xs={6} sm={6} md={6}>
                  <Stack direction="row">
                    {item?.nodeType?.toLowerCase() === dataConstant.skill ? (
                      <StarBorderOutlined
                        sx={{ mr: 1, maxWidth: 20, display: 'flex', alignSelf: 'center' }}
                      />
                    ) : item?.nodeType?.toLowerCase() === dataConstant.category ? (
                      <Folder
                        fontSize="small"
                        sx={{ mr: 1, maxWidth: 20, display: 'flex', alignSelf: 'center' }}
                      />
                    ) : null}
                    <Typography
                      sx={{
                        whiteSpace: 'nowrap',
                        overflow: 'hidden',
                        textOverflow: 'ellipsis',
                        width: '100%',
                        display: 'block',
                      }}
                    >
                      {item.name}
                    </Typography>
                  </Stack>
                </Grid>
                <Grid item xs={6} sm={6} md={6}>
                  <Stack direction="row-reverse" spacing={0}>
                    {['Total', 5, 4, 3, 2, 1].map((x, i) => {
                      {
                        var value
                        if (item.nodeType === dataConstant.Category) {
                          var list = childrenCabability(data, item.unqGUID, `${param}${i}`)
                          value = list.length > 0 ? total(list) : 0
                        } else value = item?.[`${param}${i}`]
                        return (
                          <Box
                            key={i}
                            sx={{
                              // pt: -4,
                              mt: -0.5,
                              mb: -0.5,
                              width: 55,
                              height: 35,
                              display: 'flex',
                              alignItems: 'center',
                              justifyContent: 'center',
                              border: '1px solid ',
                            }}
                          >
                            {value ? value : null}
                          </Box>
                        )
                      }
                    })}
                  </Stack>
                </Grid>
              </Grid>
            }
          >
            {createCapabilityTreeItems(data, item.unqGUID, param, detailLevel)}
          </TreeItem>
        )
      }) ?? []
  )
}
export const childrenCabability = (data, guid, param) => {
  const children = []

  // Find direct children of the given GUID
  const directChildrenCategory = data.filter((item) => item.parentID === guid)

  directChildrenCategory.forEach((child) => {
    // Recursively find children of each direct child
    var nestedChildren
    if (child.nodeType === dataConstant.Category) {
      nestedChildren = childrenCabability(data, child.unqGUID, param)
      nestedChildren.forEach((item) => children.push(item[param] ?? 0))
    } else if (child[param]) children.push(child[param])
  })

  return children
}
export const total = (list) => list.reduce((prev, curr) => prev + curr)

export const trendsConfiguration = (date, self, supervisior) => {
  return {
    labels: date,
    datasets: [
      {
        label: 'Skill level (Self assessment)',
        data: self,
        borderColor: 'rgba(54, 162, 235)',
        backgroundColor: 'rgba(54, 162, 235)',
        spanGaps: true,
        pointRadius: 5,
      },
      {
        label: 'Skill level (Supervisor assessment)',
        data: supervisior,
        borderColor: 'rgba(255, 99, 132)',
        backgroundColor: 'rgba(255, 99, 132)',
        spanGaps: true,
        pointRadius: 5,
      },
    ],
  }
}

export const getTrendSummary = (data) => {
  const result = {
    date: [],
    self: [],
    supervisior: [],
  }

  const dateSet = new Set()
  data.forEach((item) => item.date.forEach((date) => dateSet.add(date)))
  const uniqueDates = Array.from(dateSet).sort()

  result.date = uniqueDates

  // Iterate over each unique date
  uniqueDates.forEach((date) => {
    let selfSum = 0
    let selfCount = 0
    let supervisiorSum = 0
    let supervisiorCount = 0

    // Calculate the sum and count of non-null values for each date
    data.forEach((item) => {
      const dateIndex = item.date.indexOf(date)
      if (dateIndex !== -1) {
        if (item.self[dateIndex] !== null) {
          selfSum += item.self[dateIndex]
          selfCount++
        }
        if (item.supervisior[dateIndex] !== null) {
          supervisiorSum += item.supervisior[dateIndex]
          supervisiorCount++
        }
      }
    })

    // Calculate the average for self and supervisior, or return null if count is 0
    result.self.push(selfCount > 0 ? selfSum / selfCount : null)
    result.supervisior.push(supervisiorCount > 0 ? supervisiorSum / supervisiorCount : null)
  })

  return result
}
export const createSummarySkillTreeItems = (data, parentId = null, view) => {
  return (
    data
      ?.filter((item) => item.parentID === parentId)
      .map((item) => {
        return (
          <TreeItem
            key={item.unqGUID}
            itemId={item.unqGUID}
            label={
              <Grid container rowSpacing={2}>
                <Grid item xs={12} sm={6} md={6} lg={6}>
                  <Stack direction="row">
                    {item?.nodeType?.toLowerCase() === dataConstant.skill ? (
                      <StarBorderOutlined
                        sx={{ mr: 1, maxWidth: 20, display: 'flex', alignSelf: 'center' }}
                      />
                    ) : item?.nodeType?.toLowerCase() === dataConstant.category ? (
                      <Folder
                        fontSize="small"
                        sx={{ mr: 1, maxWidth: 20, display: 'flex', alignSelf: 'center' }}
                      />
                    ) : null}
                    <Typography
                      sx={{
                        whiteSpace: 'nowrap',
                        overflow: 'hidden',
                        textOverflow: 'ellipsis',
                        width: '100%',
                        display: 'block',
                      }}
                    >
                      {item.name}
                    </Typography>
                  </Stack>
                </Grid>
                <Grid item xs={12} sm={6} md={6} lg={6}>
                  <Stack direction="row-reverse" spacing={0}>
                    {[
                      { visible: view.supervisior, param: 'superSkill' },
                      { visible: view.self, param: 'selfInterest' },
                      { visible: view.self, param: 'selfSkill' },
                      { visible: view.average, param: 'avgSkill' },
                    ].map((x, i) => {
                      var value
                      if (item.nodeType === dataConstant.Category) {
                        var list = childrenAverageSummaySkill(data, item.unqGUID, x.param)
                        value = list.length > 0 ? average(list) : 0
                      } else value = item?.[x.param]
                      return x?.visible ? (
                        <Box
                          key={i}
                          sx={{
                            // pt: -4,
                            mt: -0.5,
                            mb: -0.5,
                            width: 100,
                            height: 35,
                            // borderRadius: 2,
                            display: 'flex',
                            backgroundColor: value ? getColorForRating(value) : '',
                            alignItems: 'center',
                            justifyContent: 'center',
                            // border: '1px solid ',
                          }}
                        >
                          {value ? value?.toFixed(2) : null}
                        </Box>
                      ) : null
                    })}
                  </Stack>
                </Grid>
              </Grid>
            }
          >
            {createSummarySkillTreeItems(data, item.unqGUID, view)}
          </TreeItem>
        )
      }) ?? []
  )
}
export const childrenAverageSummaySkill = (data, guid, param) => {
  const children = []

  // Find direct children of the given GUID
  const directChildrenCategory = data.filter(
    (item) => item.parentID === guid && item.nodeType === dataConstant.Category,
  )
  const directChildrenSkill = data.filter(
    (item) => item.parentID === guid && item.nodeType === dataConstant.Skill,
  )

  directChildrenCategory.forEach((child) => {
    // Recursively find children of each direct child
    var nestedChildren = childrenAverageSummaySkill(data, child.unqGUID, param)
    nestedChildren.forEach((item) => children.push(item))
  })

  directChildrenSkill.forEach((child) => {
    if (child[param]) children.push(child[param])
  })

  return children
}

export const updateSummarySkill = (data, parentID = null) => {
  // Filter the nodes where the parentID matches the current node's parentID
  const children = data.filter((node) => node.parentID === parentID)

  children.forEach((node) => {
    // Recursively process the child nodes
    updateSummarySkill(data, node.unqGUID)

    // Find the child nodes of the current node after recursion
    const childNodes = data.filter((child) => child.parentID === node.unqGUID)

    // Check if any child has activeSkill or assignedSkill = 1
    const hasActiveChild = childNodes.some((child) => child.activeSkill === 1)
    const hasAssignedChild = childNodes.some((child) => child.assignedSkill === 1)

    // Update the current node's activeSkill and assignedSkill if any child has value 1
    if (hasActiveChild) node.activeSkill = 1
    if (hasAssignedChild) node.assignedSkill = 1
  })
}
