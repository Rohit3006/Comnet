using System.ComponentModel.DataAnnotations;

namespace Comnet.Data.DBModels
{
    public class Car : BaseModel
    {
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
        public DateTime DateOfManufactring { get; set; }

        public bool IsActive { get; set; }

        public decimal? SortOrder { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Feature { get; set; } = string.Empty;

        public virtual ICollection<Images> Images { get; set; } = new HashSet<Images>();
    }
}