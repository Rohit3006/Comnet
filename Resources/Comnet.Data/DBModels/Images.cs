using System.ComponentModel.DataAnnotations;

namespace Comnet.Data.DBModels
{
    public class Images : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Url { get; set; } = "";

        public int Order { get; set; }

        [Required]
        public Guid CarID { get; set; }

    }
}