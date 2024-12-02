using Comnet.Common.Model;
using Comnet.Data.Contracts.ViewModels.Delete;
using Comnet.Data.Contracts.ViewModels.Grid;
using Comnet.Data.Contracts.ViewModels.Car;
using Comnet.Data.SPModels.Car;

namespace Comnet.Business.Contracts
{
    public interface ICarManager
    {
        Task<APIResponse<String>> AddCar(CarRequest request);
        Task<APIResponse<String>> UpdateCar(CarRequest request);
        Task<APIResponse<CarDetails>> GetCarById(Guid id);
        Task<APIResponse<String>> DeleteCar(DeleteRequest request);
        Task<APIResponse<GenericGridVM<CarList>>> GetCarList(PagingInfoVM request);
    }
}
