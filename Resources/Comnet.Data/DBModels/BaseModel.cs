using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Comnet.Data.DBModels
{
    public class BaseModel
    {
        /// <summary>
        /// Primary of creation of data
        /// </summary>
        [Key]
        [Required]
        public Guid UnqGUID { get; set; }

        /// <summary>
        /// Soft delete of data
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Date and Time of creation of data
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Email of person who is creating data
        /// </summary>
        [StringLength(128)]
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Date and Time of updating of data
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Email of person who is updating data
        /// </summary>
        [StringLength(128)]
        public string? UpdatedBy { get; set; }
    }
}
