using System.ComponentModel.DataAnnotations;

namespace Comnet.Data.Contracts.ViewModels.Grid
{
    public class PagingInfoVM
    {
        /// <summary>
        /// Page number for grid
        /// </summary>
        public Guid? UnqGUID { get; set; }
        /// <summary>
        /// Page number for grid
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }
        /// <summary>
        /// Rows per page for grid
        /// </summary>
        [Range(1, int.MaxValue)]
        public int RowsPerPage { get; set; }
        /// <summary>
        /// Columns for searching
        /// </summary>
        public List<SearchColumn>? SearchColumns { get; set; }
        /// <summary>
        /// Columns for sorting
        /// </summary>
        public SortColumn? SortColumns { get; set; }
        /// <summary>
        /// Search text
        /// </summary>
        public string? SearchText { get; set; }
    }

    public class SearchColumn
    {
        public string? SearchedColumn { get; set; }
        public string? SearchText { get; set; }
        public string? ColumnDataType { get; set; }
    }
    public class SortColumn
    {
        public string? SortColumnName { get; set; }
        public string? SortOrder { get; set; }
    }
}
