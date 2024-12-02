using Comnet.Business.Contracts;
using Comnet.Data.Contracts.RepostoryInterfaces;
using System.Net;
using Comnet.Common.Model;
using Comnet.Data.Contracts.ViewModels.Car;
using Comnet.Data.Contracts.ViewModels.Delete;
using Comnet.Data.Contracts.ViewModels.Grid;
using Comnet.Data.DBModels;
using Comnet.Common.Helpers;
using Microsoft.AspNetCore.Hosting;
using Comnet.Data.SPModels.Car;
using Microsoft.EntityFrameworkCore;
using BookingEngine.Data.Contracts.ViewModels.MediaDetails;

namespace Comnet.Business.Manager.Managers
{
    public class CarManager : ICarManager
    {
        private readonly ICarRepository _iCarRepository;
        private readonly ICarImageRepository _iCarImageRepository;
        private readonly ContextHelper _helper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarManager(
            ICarRepository iCarRepository,
            ContextHelper helper,
            IWebHostEnvironment webHostEnvironment,
            ICarImageRepository carImageRepository)
        {
            _iCarRepository = iCarRepository;
            _helper = helper;
            _webHostEnvironment = webHostEnvironment;
            _iCarImageRepository = carImageRepository;
        }
        public async Task<APIResponse<String>> AddCar(CarRequest request)
        {
            #region Validations
            var objCar = await _iCarRepository.FindOneAysnc(x => x.Name == request.Name && !x.IsDeleted);
            if (objCar != null)
            {
                return new APIResponse<string>(HttpStatusCode.OK, APIStatus.Exists, null, null, null);
            }
            #endregion

            Car Car = new()
            {
                UnqGUID = Guid.NewGuid(),
                Name = request.Name,
                Code = request.Code,
                Brand = request.Brand,
                Class = request.Class,
                DateOfManufactring = Convert.ToDateTime(request.ManufacturingDate),
                Description = request.Description,
                Feature = request.Features,
                IsActive = request.IsActive,
                Price = request.Price,
                SortOrder = request.SortOrder,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _helper.GetUserIDFromToken()
            };

            if (request.Files != null && request.Files.Count > 0)
            {
                var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                var fileUrls = new List<string>();

                foreach (var file in request.Files)
                {
                    if (file.ImageFile != null)
                    {
                        // Generate a unique filename for each file to avoid overwriting
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.ImageFile?.FileName);
                        var filePath = Path.Combine(uploadsDirectory, fileName);

                        // Save the file to the specified path
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            if (file.ImageFile != null)
                            {
                                await file.ImageFile.CopyToAsync(fileStream);
                            }
                        }

                        _iCarImageRepository.Add(new Images()
                        {
                            UnqGUID = Guid.NewGuid(),
                            Url = $"\\uploads\\{fileName}",
                            CarID = Car.UnqGUID,
                            Order = file.ImageOrder,
                            IsDeleted = false,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = _helper.GetUserIDFromToken()
                        });
                    }
                }
            }
            _iCarRepository.Add(Car);
            await _iCarRepository.SaveChangesAysnc();
            return new APIResponse<string>(HttpStatusCode.OK, APIStatus.Success, null, null, null);
        }
        public async Task<APIResponse<String>> UpdateCar(CarRequest request)
        {
            #region Validations
            Car objCarExists = await _iCarRepository.FindOneAysnc(x => x.UnqGUID != request.UnqGUID && x.Name == request.Name && !x.IsDeleted);
            if (objCarExists != null)
            {
                return new APIResponse<string>(HttpStatusCode.OK, APIStatus.Exists, null, null, null);
            }
            Car Car = await _iCarRepository.FindOneAysnc(x => x.UnqGUID == request.UnqGUID && !x.IsDeleted);
            if (Car == null)
            {
                return new APIResponse<string>(HttpStatusCode.OK, APIStatus.NotFound, null, null, null);
            }
            #endregion

            Car.Name = request.Name;
            Car.Code = request.Code;
            Car.Brand = request.Brand;
            Car.Class = request.Class;
            Car.DateOfManufactring = Convert.ToDateTime(request.ManufacturingDate);
            Car.Description = request.Description;
            Car.Feature = request.Features;
            Car.IsActive = request.IsActive;
            Car.Price = request.Price;
            Car.SortOrder = request.SortOrder;
            Car.UpdatedDate = DateTime.UtcNow;
            Car.UpdatedBy = _helper.GetUserIDFromToken();

            if (request.Files != null && request.Files.Count > 0)
            {
                foreach (var file in request.Files)
                {
                    if (file.UnqGUID == null)
                    {
                        if (file.ImageFile != null)
                        {
                            var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                            // Generate a unique filename for each file to avoid overwriting
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.ImageFile?.FileName);
                            var filePath = Path.Combine(uploadsDirectory, fileName);

                            // Save the file to the specified path
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                if (file.ImageFile != null)
                                {
                                    await file.ImageFile.CopyToAsync(fileStream);
                                }
                            }

                            _iCarImageRepository.Add(new Images()
                            {
                                UnqGUID = Guid.NewGuid(),
                                Url = $"\\uploads\\{fileName}",
                                CarID = Car.UnqGUID,
                                Order = file.ImageOrder,
                                IsDeleted = false,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = _helper.GetUserIDFromToken()
                            });
                        }
                    }
                    else
                    {
                        Images images = await _iCarImageRepository.FindOneAysnc(x => x.UnqGUID == file.UnqGUID && !x.IsDeleted);
                        images.IsDeleted = file.IsDeleted;
                        images.Order = file.ImageOrder;
                        images.UpdatedDate = DateTime.UtcNow;
                        images.UpdatedBy = _helper.GetUserIDFromToken();
                        _iCarImageRepository.Update(images);
                    }
                }
            }


            _iCarRepository.Update(Car);
            await _iCarRepository.SaveChangesAysnc();
            return new APIResponse<string>(HttpStatusCode.OK, APIStatus.Success, null, null, null);
        }
        public async Task<APIResponse<CarDetails>> GetCarById(Guid Id)
        {
            Car Car = await _iCarRepository.FindOneAysnc(x => x.UnqGUID == Id && !x.IsDeleted);
            if (Car == null)
            {
                return new APIResponse<CarDetails>(HttpStatusCode.OK, APIStatus.NotFound, null, null, null);
            }

            List<Images> Images = await _iCarImageRepository.Find(x => x.CarID == Car.UnqGUID && !x.IsDeleted).ToListAsync();
            List<ImagesDetails> details = new();
            foreach (Images image in Images)
            {
                details.Add(new ImagesDetails()
                {
                    UnqGUID = image.UnqGUID,
                    ImagesUrl = image.Url,
                    IsDeleted = image.IsDeleted,
                });
            }

            CarDetails CarDetails = new()
            {
                UnqGUID = Car.UnqGUID,
                Name = Car.Name,
                Code = Car.Code,
                Brand = Car.Brand,
                Class = Car.Class,
                ManufacturingDate = Car.DateOfManufactring,
                Description = Car.Description,
                Features = Car.Feature,
                IsActive = Car.IsActive,
                Price = Car.Price,
                SortOrder = Car.SortOrder,
                Files = details,
            };

            return new APIResponse<CarDetails>(HttpStatusCode.OK, APIStatus.Success, CarDetails, null, null);
        }
        public async Task<APIResponse<String>> DeleteCar(DeleteRequest request)
        {
            Car Car = await _iCarRepository.FindOneAysnc(x => x.UnqGUID == request.Guid && !x.IsDeleted);
            if (Car == null)
            {
                return new APIResponse<String>(HttpStatusCode.OK, APIStatus.NotFound, null, null, null);
            }
            List<Images> Images = await _iCarImageRepository.Find(x => x.CarID == Car.UnqGUID && !x.IsDeleted).ToListAsync();

            Car.IsDeleted = true;
            Car.UpdatedDate = DateTime.UtcNow;
            Car.UpdatedBy = _helper.GetUserIDFromToken();

            foreach (Images image in Images)
            {
                image.IsDeleted = true;
                image.UpdatedDate = DateTime.UtcNow;
                image.UpdatedBy = _helper.GetUserIDFromToken();
                _iCarImageRepository.Update(image);
            }

            _iCarRepository.Update(Car);
            await _iCarRepository.SaveChangesAysnc();
            return new APIResponse<String>(HttpStatusCode.OK, APIStatus.Success, null, null, null);
        }
        public async Task<APIResponse<GenericGridVM<CarList>>> GetCarList(PagingInfoVM request)
        {
            var result = await _iCarRepository.GetCarList(request);
            return new APIResponse<GenericGridVM<CarList>>(HttpStatusCode.OK, APIStatus.Success, result, null, null);
        }
    }
}