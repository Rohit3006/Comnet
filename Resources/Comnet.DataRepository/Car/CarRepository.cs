using Comnet.Data.Context;
using Comnet.Data.Contracts.RepostoryInterfaces;
using Comnet.Data.Contracts.ViewModels.Grid;
using Comnet.Data.DBModels;
using Comnet.Data.SPModels.Car;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlTypes;

namespace Comnet.DataRepository
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(ComnetDbContext _context) : base(_context) { }

        public async Task<GenericGridVM<CarList>> GetCarList(PagingInfoVM request)
        {
            string sortColumns = "";
            int totalCount = 0;

            if (request.SortColumns != null && !string.IsNullOrEmpty(request.SortColumns.SortColumnName) &&
               !string.IsNullOrEmpty(request.SortColumns.SortOrder))
            {
                sortColumns = request.SortColumns.SortColumnName;
                sortColumns += string.Format(" {0}", request.SortColumns.SortOrder);
            }

            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@pPageNumber", request.PageNumber),
                new SqlParameter("@pRowsPerPage", request.RowsPerPage),
                new SqlParameter("@pOrderBy", !string.IsNullOrEmpty(sortColumns) ? sortColumns : SqlString.Null),
                new SqlParameter("@pKeyWord",  request.SearchText ?? SqlString.Null),
                new SqlParameter("@pTotalCount", totalCount)
                {
                    Direction = ParameterDirection.Output,
                    SqlDbType = SqlDbType.Int
                }
            };

            List<CarList> list = await _context.Set<CarList>().FromSqlRaw("Prc_GetCarList @pPageNumber, @pRowsPerPage, @pOrderBy, @pKeyWord, @pTotalCount OUT", sqlParameters).ToListAsync();
            GenericGridVM<CarList> genericGridVM = new GenericGridVM<CarList>
            {
                List = list.AsQueryable(),
                TotalCount = (int)sqlParameters[sqlParameters.Length - 1].Value
            };

            return genericGridVM;
        }
    }
}
