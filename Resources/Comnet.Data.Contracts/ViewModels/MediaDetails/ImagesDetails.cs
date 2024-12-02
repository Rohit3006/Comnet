namespace BookingEngine.Data.Contracts.ViewModels.MediaDetails
{
    public class ImagesDetails
    {
        public Guid? UnqGUID { get; set; }
        public string ImagesUrl { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}
