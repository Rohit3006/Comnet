using System.Data.Common;
using System.Linq.Expressions;

namespace Comnet.Data.Contracts.RepostoryInterfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> All();

        T GetById(long id);
        void SaveChanges();
        Task<int> SaveChangesAysnc();
        int Delete(T T);
        bool Contains(Expression<Func<T, bool>> predicate);
        T FindOne(params object[] keys);
        Task<T> FindOneAysnc(params object[] keys);
        T FindOne(Expression<Func<T, bool>> predicate);
        Task<T> FindOneAysnc(Expression<Func<T, bool>> predicate);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        T Add(T t);
        Task AddRangeAsync(IEnumerable<T> entities);
        int Delete(int id);
        int Delete(Expression<Func<T, bool>> predicate);
        int Update(T t);
        void ExecuteSqlCommand(string sql, params object[] parameters);
        void SetValues(object DestinationValue, object SourceValue);

        DbCommand GetCommand();
        void Reload(T t);
        void ReloadReference(T t, string refProperty);

        bool Any(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    }
}
