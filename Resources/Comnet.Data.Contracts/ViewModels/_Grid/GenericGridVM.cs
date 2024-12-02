namespace Comnet.Data.Contracts.ViewModels.Grid
{
    public class GenericGridVM<T>
    {
        /// <summary>
        /// Records for grid
        /// </summary>
        public IQueryable<T>? List { get; set; }
        /// <summary>
        /// Total counts of grid
        /// </summary>
        public int TotalCount { get; set; } = 0;
    }
}
