namespace Comnet.Data.Contracts.ViewModels.Delete
{
    public class DeleteRequest
    {
        /// <summary>
        /// GUID of item to be deleted
        /// </summary>
        public Guid? Guid { get; set; }
        /// <summary>
        /// Deleted by
        /// </summary>
        public string? DeletedBy { get; set; }
    }
}
