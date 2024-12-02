using BookingEngine.Data.Contracts.ViewModels.MediaDetails;

namespace Comnet.Data.Contracts.ViewModels.Car
{
    public class CarDetails
    {
        public Guid? UnqGUID { get; set; }

        public string Name { get; set; } = "";

        public string Code { get; set; } = "";

        public string Brand { get; set; } = "";

        public string Class { get; set; } = "";

        public decimal Price { get; set; }

        public DateTime ManufacturingDate { get; set; }

        public bool IsActive { get; set; }

        public decimal? SortOrder { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Features { get; set; } = string.Empty;

        public virtual ICollection<ImagesDetails>? Files { get; set; } = new HashSet<ImagesDetails>();
    }
}
