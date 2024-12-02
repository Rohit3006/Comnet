using Comnet.Data.Context;
using Comnet.Data.Contracts.RepostoryInterfaces;
using Comnet.Data.DBModels;

namespace Comnet.DataRepository
{
    public class CarImageRepository : Repository<Images>, ICarImageRepository
    {
        public CarImageRepository(ComnetDbContext _context) : base(_context) { }
    }
}
