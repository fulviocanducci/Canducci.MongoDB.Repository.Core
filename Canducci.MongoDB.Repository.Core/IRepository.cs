using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Canducci.MongoDB.Repository.Core
{
    public interface IRepository<T> : IDisposable
        where T : class, new()
    {
        IConnect Connect { get; }
        IMongoCollection<T> Collection { get; }
        string CollectionName { get; }

        T Add(T model);
        IEnumerable<T> Add(IEnumerable<T> models);
        Task<T> AddAsync(T model);
        Task<IEnumerable<T>> AddAsync(IEnumerable<T> models);

        bool Edit(Expression<Func<T, bool>> where, T model);
        Task<bool> EditAsync(Expression<Func<T, bool>> where, T model);
        bool Edit(FilterDefinition<T> filter, T model);
        Task<bool> EditAsync(FilterDefinition<T> filter, T model);

        bool Update(Expression<Func<T, bool>> where, UpdateDefinition<T> update);
        bool UpdateAll(Expression<Func<T, bool>> where, UpdateDefinition<T> update);
        Task<bool> UpdateAsync(Expression<Func<T, bool>> where, UpdateDefinition<T> update);
        Task<bool> UpdateAllAsync(Expression<Func<T, bool>> where, UpdateDefinition<T> update);

        bool Update(FilterDefinition<T> filter, UpdateDefinition<T> update);
        bool UpdateAll(FilterDefinition<T> filter, UpdateDefinition<T> update);
        Task<bool> UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);
        Task<bool> UpdateAllAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);

        T Find(Expression<Func<T, bool>> where);
        Task<T> FindAsync(Expression<Func<T, bool>> where);
        T Find(FilterDefinition<T> filter);
        Task<T> FindAsync(FilterDefinition<T> filter);

        IEnumerable<T> All();
        IEnumerable<T> All(Expression<Func<T, bool>> where);
        IEnumerable<T> All<Tkey>(Expression<Func<T, bool>> where, Expression<Func<T, Tkey>> orderBy);
        Task<IList<T>> AllAsync();
        Task<IList<T>> AllAsync(Expression<Func<T, bool>> where);
        Task<IList<T>> AllAsync<Tkey>(Expression<Func<T, bool>> where, Expression<Func<T, Tkey>> orderBy);

        IEnumerable<T> All(FilterDefinition<T> filter);
        IEnumerable<T> All(FilterDefinition<T> filter, SortDefinition<T> sort);        
        Task<IList<T>> AllAsync(FilterDefinition<T> filter);
        Task<IList<T>> AllAsync(FilterDefinition<T> filter, SortDefinition<T> sort);

        IList<T> List(SortDefinition<T> sort);
        Task<IList<T>> ListAsync(SortDefinition<T> sort);
        IList<T> List(SortDefinition<T> sort, FilterDefinition<T> filter);
        Task<IList<T>> ListAsync(SortDefinition<T> sort, FilterDefinition<T> filter);

        IList<T> List<Tkey>(Expression<Func<T, Tkey>> orderBy);
        Task<IList<T>> ListAsync<Tkey>(Expression<Func<T, Tkey>> orderBy);
        IList<T> List<Tkey>(Expression<Func<T, Tkey>> orderBy, Expression<Func<T, bool>> where);
        Task<IList<T>> ListAsync<Tkey>(Expression<Func<T, Tkey>> orderBy, Expression<Func<T, bool>> where);

        bool Delete(Expression<Func<T, bool>> where);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> where);
        bool Delete(FilterDefinition<T> filter);
        Task<bool> DeleteAsync(FilterDefinition<T> filter);

        bool DeleteAll(Expression<Func<T, bool>> where);
        Task<bool> DeleteAllAsync(Expression<Func<T, bool>> where);
        bool DeleteAll(FilterDefinition<T> filter);
        Task<bool> DeleteAllAsync(FilterDefinition<T> filter);

        IMongoQueryable<T> Query();
         
        ObjectId CreateObjectId(string value);

        long Count();
        long Count(Expression<Func<T, bool>> where, CountOptions options = null);
        long Count(FilterDefinition<T> filter, CountOptions options = null);
        Task<long> CountAsync();
        Task<long> CountAsync(Expression<Func<T, bool>> where, CountOptions options = null);
        Task<long> CountAsync(FilterDefinition<T> filter, CountOptions options = null);
    }
}
