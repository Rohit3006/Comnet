using Comnet.Data.Contracts.ViewModels.Grid;
using Comnet.Data.DBModels;
using Comnet.Data.SPModels.Car;

namespace Comnet.Data.Contracts.RepostoryInterfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<GenericGridVM<CarList>> GetCarList(PagingInfoVM request);
    }
}
