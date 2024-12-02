using BookingEngine.Data.Contracts.ViewModels.MediaDetails;
using System.ComponentModel.DataAnnotations;

namespace Comnet.Data.Contracts.ViewModels.Car
{
    public class CarRequest
    {
        public Guid? UnqGUID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Code { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Brand { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Class { get; set; } = "";

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ManufacturingDate { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public decimal? SortOrder { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Features { get; set; } = string.Empty;
        public virtual ICollection<ImagesRequest>? Files { get; set; }

        //public List<IFormFile> Files { get; set; } = [];
    }
}
