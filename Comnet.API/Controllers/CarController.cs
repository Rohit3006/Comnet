using Comnet.Business.Contracts;
using Comnet.Common.Model;
using Comnet.Data.Contracts.ViewModels.Delete;
using Comnet.Data.Contracts.ViewModels.Grid;
using Microsoft.AspNetCore.Mvc;
using Comnet.Data.Contracts.ViewModels.Car;
using Comnet.Data.SPModels.Car;

namespace Comnet.API.Controllers
{
    /// <summary>
    /// Controller Endpoint
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarManager _iCarManager;

        /// <summary>
        /// Dependancy Injection
        /// </summary>
        public CarController(ICarManager iCarManager)
        {
            _iCarManager = iCarManager;
        }

        /// <summary>
        /// To add new Language.
        /// </summary>
        /// <param name="request">Request parameter with Language information.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResponse<String>> AddCar([FromForm] CarRequest request)
        {
            var result = await _iCarManager.AddCar(request);
            return result;
        }

        /// <summary>
        /// To update an existing Language.
        /// </summary>
        /// <param name="request">Request parameter with Language information.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResponse<String>> UpdateCar([FromForm] CarRequest request)
        {
            var result = await _iCarManager.UpdateCar(request);
            return result;
        }

        /// <summary>
        /// To get Language by Id.
        /// </summary>
        /// <param name="id">Request parameter with Language information.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<APIResponse<CarDetails>> GetCarById(Guid id)
        {
            var result = await _iCarManager.GetCarById(id);
            return result;
        }

        /// <summary>
        /// To delete Language by Id.
        /// </summary>
        /// <param name="request">Request parameter with Language information.</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<APIResponse<String>> DeleteCar(DeleteRequest request)
        {
            var result = await _iCarManager.DeleteCar(request);
            return result;
        }

        /// <summary>
        /// Get Language List
        /// </summary>
        /// <param name="request">Request parameter with Language grid information.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResponse<GenericGridVM<CarList>>> GetCarList(PagingInfoVM request)
        {
            var result = await _iCarManager.GetCarList(request);
            return result;
        }
    }
}
