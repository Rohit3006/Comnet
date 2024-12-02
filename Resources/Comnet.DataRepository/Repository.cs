using Comnet.Data.Context;
using Comnet.Data.Contracts.RepostoryInterfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace Comnet.DataRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ComnetDbContext _context = null!;

        public Repository(ComnetDbContext context)
        {
            this._context = context;
        }

        protected DbSet<T> DbSet
        {
            get
            {
                return _context.Set<T>();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public virtual T GetById(long id)
        {
            var record = DbSet.Find(id);
            if (record == null) {
                return null!;
            }
            return record;
        }

        public virtual IQueryable<T> All()
        {
            return DbSet.AsQueryable();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveChangesAysnc()
        {
            return await _context.SaveChangesAsync();
        }

        public bool Contains(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        public virtual T FindOne(params object[] keys)
        {
            var record = DbSet.Find(keys);
            if (record == null) {
                return null!;
            }
            return record;
        }
        public virtual async Task<T> FindOneAysnc(params object[] keys)
        {
            var record = await DbSet.FindAsync(keys);
            if (record == null) {
                return null!;
            }
            return record;
        }

        public virtual T FindOne(Expression<Func<T, bool>> predicate)
        {
            var record = DbSet.FirstOrDefault(predicate);
            if (record == null) {
                return null!;
            }
            return record;
        }
        public virtual async Task<T> FindOneAysnc(Expression<Func<T, bool>> predicate)
        {
            var record = await DbSet.FirstOrDefaultAsync(predicate);
            if (record == null) {
                return null!;
            }
            return record;
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
        public virtual async Task<IQueryable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await DbSet.Where(predicate).ToListAsync();
            return result.AsQueryable();
        }

        public virtual T Add(T T)
        {
            var newEntry = DbSet.Add(T);
            return newEntry.Entity;
        }

        public virtual Task AddRangeAsync(IEnumerable<T> entities)
        {
            var newEntryRange = DbSet.AddRangeAsync(entities);
            return newEntryRange;
        }
        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public virtual int Delete(int id)
        {
            var model = GetById(id);
            DbSet.Remove(model);
            return 0;
        }

        public virtual int Update(T T)
        {
            var entry = _context.Entry(T);
            DbSet.Attach(T);
            entry.State = EntityState.Modified;
            return 0;
        }

        public virtual int Delete(Expression<Func<T, bool>> predicate)
        {
            var objects = Find(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);
            return 0;
        }

        public virtual void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            //TODO _context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public virtual void SetValues(object DestinationValue, object SourceValue)
        {
            _context.Entry(DestinationValue).CurrentValues.SetValues(SourceValue);
        }


        public virtual DbCommand GetCommand()
        {
            DbCommand dbCmd = _context.Database.GetDbConnection().CreateCommand();
            return dbCmd;
        }

        public virtual void Reload(T T)
        {
            _context.Entry(T).Reload();
        }

        public virtual void ReloadReference(T T, string refProperty)
        {
            _context.Entry(T).Reference(refProperty).Load();
        }

        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
        }

        public virtual int Delete(T T)
        {
            DbSet.Remove(T);
            //Commit();
            return 0;
        }

        public DbCommand LoadStoredProc(DbContext context, string storedProcName, SqlParameter[] parameters, bool prependDefaultSchema = true)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            if (prependDefaultSchema)
            {
                var schemaName = "dbo";
                if (schemaName != null)
                {
                    storedProcName = $"{schemaName}.{storedProcName}";
                }

            }
            cmd.CommandText = storedProcName;
            cmd.Parameters.AddRange(parameters);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }

        public void ExecuteStoredProc(DbCommand command, Action<SprocResults> handleResults, System.Data.CommandBehavior commandBehaviour = System.Data.CommandBehavior.Default, bool manageConnection = true)
        {
            if (handleResults == null)
            {
                throw new ArgumentNullException(nameof(handleResults));
            }

            using (command)
            {
                if (manageConnection && command.Connection!.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    using (var reader = command.ExecuteReader(commandBehaviour))
                    {
                        var sprocResults = new SprocResults(reader);
                        handleResults(sprocResults);
                    }
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection!.Close();
                    }
                }
            }
        }

        public async Task ExecuteStoredProcedureAsync(DbCommand command, Action<SprocResults> handleResults, System.Data.CommandBehavior commandBehaviour = System.Data.CommandBehavior.Default, bool manageConnection = true)
        {
            if (handleResults == null)
            {
                throw new ArgumentNullException(nameof(handleResults));
            }

            using (command)
            {
                if (manageConnection && command.Connection!.State == System.Data.ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    using (var reader = await command.ExecuteReaderAsync(commandBehaviour))
                    {
                        var sprocResults = new SprocResults(reader);
                        handleResults(sprocResults);
                    }
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection!.Close();
                    }
                }
            }
        }

        public class SprocResults
        {

            //  private DbCommand _command;
            private DbDataReader _reader;

            public SprocResults(DbDataReader reader)
            {
                // _command = command;
                _reader = reader;
            }

            public IList<U> ReadToList<U>()
            {
                return MapToList<U>(_reader);
            }

            public U? ReadToValue<U>() where U : struct
            {
                return MapToValue<U>(_reader);
            }

            public Task<bool> NextResultAsync()
            {
                return _reader.NextResultAsync();
            }

            public Task<bool> NextResultAsync(CancellationToken ct)
            {
                return _reader.NextResultAsync(ct);
            }

            public bool NextResult()
            {
                return _reader.NextResult();
            }

            /// <summary>
            /// Retrieves the column values from the stored procedure and maps them to <typeparamref name="T"/>'s properties
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="dr"></param>
            /// <returns>IList<<typeparamref name="T"/>></returns>
            private IList<U> MapToList<U>(DbDataReader dr)
            {
                var objList = new List<U>();
                var props = typeof(U).GetRuntimeProperties().ToList();

                var colMapping = dr.GetColumnSchema()
                    .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
                    .ToDictionary(key => key.ColumnName.ToLower());

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        U obj = Activator.CreateInstance<U>();
                        foreach (var prop in props)
                        {
                            if (colMapping.ContainsKey(prop.Name.ToLower()))
                            {
                                var column = colMapping[prop.Name.ToLower()];

                                if (column?.ColumnOrdinal != null)
                                {
                                    var val = dr.GetValue(column.ColumnOrdinal.Value);
                                    if (prop.PropertyType == typeof(DateOnly))
                                    {
                                        prop.SetValue(obj, val == DBNull.Value ? null : DateOnly.FromDateTime((DateTime)val));
                                        
                                    }
                                    else if (prop.PropertyType == typeof(TimeOnly))
                                    {
                                        prop.SetValue(obj, val == DBNull.Value ? null : TimeOnly.FromDateTime((DateTime)val));

                                    }
                                    else
                                     prop.SetValue(obj, val == DBNull.Value ? null : val);
                                }
                            }
                        }
                        objList.Add(obj);
                    }
                }
                return objList;
            }

            /// <summary>
            ///Attempts to read the first value of the first row of the resultset.
            /// </summary>
            private U? MapToValue<U>(DbDataReader dr) where U : struct
            {
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        return dr.IsDBNull(0) ? new U?() : new U?(dr.GetFieldValue<U>(0));
                    }
                }
                return new U?();
            }
        }
    }
}
