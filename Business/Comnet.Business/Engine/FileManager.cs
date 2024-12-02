//using Azure.Storage.Blobs;
//using Azure.Storage.Blobs.Models;
//using Comnet.Data.DBModels;

//namespace Comnet.Business.Engine
//{
//    public class FileManager : IFileManager
//    {
//        public string storagefile = "";
//        private readonly BlobServiceClient _blobServiceClient;
//        public FileManager(BlobServiceClient blobServiceClient)
//        {
//            _blobServiceClient = blobServiceClient;
//        }
//        public async Task Upload(MediaDetails model)
//        {
//            if (model.ImageFile != null) 
//            {
//                BlobContainerClient blobContainer = _blobServiceClient.GetBlobContainerClient("media");
//                string path = model.BaseFolder + "/";
//                if (model.FileName != null)
//                {
//                    storagefile = Path.Combine(path, model.FileName + model.FileExtension);
//                }
//                BlobClient blobClient = blobContainer.GetBlobClient(storagefile);
//                BlobHttpHeaders blobHttpHeader = new() { ContentType = model.ImageFile.ContentType };
//                await blobClient.UploadAsync(model.ImageFile.OpenReadStream(), new BlobUploadOptions { HttpHeaders = blobHttpHeader });
//            }
//        }
//        public string Get(MediaDetails model)
//        {
//            var blobContainer = _blobServiceClient.GetBlobContainerClient("media");
//            var path = model.BaseFolder + "/";
//            if (model.FileName != null)
//            {
//                storagefile = Path.Combine(path, model.FileName + model.FileExtension);
//            }
//            var blobClient = blobContainer.GetBlobClient(storagefile);
//            var blobUrl = blobClient.Uri.AbsoluteUri;
//            return blobClient.Uri.AbsoluteUri;
//        }
//        public async Task Delete(MediaDetails model)
//        {
//                var blobContainer = _blobServiceClient.GetBlobContainerClient("media");
//                var path = model.BaseFolder + "/";
//                if (model.FileName != null)
//                {
//                    storagefile = Path.Combine(path, model.FileName + model.FileExtension);
//                }
//                var blobClient = blobContainer.GetBlobClient(storagefile);
//                await blobClient.DeleteIfExistsAsync();
//        }
//        public string GetBlobUrl(string url)
//        {
//            if (!string.IsNullOrEmpty(url))
//            {
//                BlobContainerClient blobContainer = _blobServiceClient.GetBlobContainerClient("media");
//                BlobClient blobClient = blobContainer.GetBlobClient(url);
//                return blobClient.Uri.AbsoluteUri;
//            }
//            return "";
//        }
//    }
//}
