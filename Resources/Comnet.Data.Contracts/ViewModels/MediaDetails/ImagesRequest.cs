using Microsoft.AspNetCore.Http;

namespace BookingEngine.Data.Contracts.ViewModels.MediaDetails
{
    public class ImagesRequest
    {
        /// <summary>
        /// Unique GUID of the Image
        /// </summary>
        public Guid? UnqGUID { get; set; }

        /// <summary>
        /// ImageFile to send image file to the Image
        /// </summary>
        public IFormFile? ImageFile { get; set; }

        /// <summary>
        /// IsDeleted flag of the Image
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// To manage order of images
        /// </summary>
        public int ImageOrder { get; set; }
    }
}
