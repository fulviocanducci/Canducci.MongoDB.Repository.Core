using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Reflection;

namespace Canducci.MongoDB.Repository.Core
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, new()
    {
        public IConnect Connect { get; private set; }
        public IMongoCollection<T> Collection { get; private set; }
        public string CollectionName { get; private set; }

        public Repository(IConnect connect)
        {
            Connect = connect;
            BuilderSetCollectionName();
            Collection = Connect.Collection<T>(CollectionName);
        }

        #region add 
        
        public T Add(T model)
        {
            Collection.InsertOne(model);
            return model;
        }

        public IEnumerable<T> Add(IEnumerable<T> models)
        {
            Collection.InsertMany(models);
            return models;
        }

        public async Task<T> AddAsync(T model)
        {
            await Collection.InsertOneAsync(model);
            return model;
        }

        public async Task<IEnumerable<T>> AddAsync(IEnumerable<T> models)
        {
            await Collection.InsertManyAsync(models);
            return models;
        }

        #endregion

        #region edit 
        
        public bool Edit(Expression<Func<T, bool>> filter, T model)
        {
            return Collection
                .ReplaceOne(filter, model)
                .ModifiedCount > 0;
        }

        public async Task<bool> EditAsync(Expression<Func<T, bool>> filter, T model)
        {
            ReplaceOneResult result = 
                await Collection
                .ReplaceOneAsync(filter, model);
            return result
                .ModifiedCount > 0;
        }

        #endregion

        #region update
                
        public bool Update(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
        {
            return Collection
                .UpdateOne(filter, update)
                .ModifiedCount > 0;
        }

        public bool UpdateAll(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
        {
            return Collection
               .UpdateMany(filter, update)
               .ModifiedCount > 0;
        }

        public async Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
        {
            UpdateResult result = await Collection
                .UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateAllAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
        {
            UpdateResult result = await Collection
                .UpdateManyAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        #endregion

        #region find

        public T Find(Expression<Func<T, bool>> filter)
        {
            return Collection
                .Find(filter)
                .FirstOrDefault();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> filter)
        {
            IAsyncCursor<T> result = await Collection
               .FindAsync(filter);
            return result
                .FirstOrDefault();
        }

        #endregion

        #region all
        
        public IEnumerable<T> All()
        {
            return Collection
                .AsQueryable()                
                .AsEnumerable();
        }

        public IEnumerable<T> All(Expression<Func<T, bool>> filter)
        {
            return Collection
                .AsQueryable()
                .Where(filter)
                .AsEnumerable();
        }

        public IEnumerable<T> All<Tkey>(Expression<Func<T, bool>> filter, Expression<Func<T, Tkey>> orderBy)
        {
            return Collection
                .AsQueryable()
                .Where(filter)
                .OrderBy(orderBy)
                .AsEnumerable();
        }

        public async Task<IList<T>> AllAsync()
        {
            return await Collection
              .AsQueryable()
              .ToListAsync();

        }

        public async Task<IList<T>> AllAsync(Expression<Func<T, bool>> filter)
        {
            return await Collection
                .AsQueryable()
                .Where(filter)
                .ToListAsync();
        }

        public async Task<IList<T>> AllAsync<Tkey>(Expression<Func<T, bool>> filter, Expression<Func<T, Tkey>> orderBy)
        {               
            return await Collection    
                .AsQueryable()
                .Where(filter)
                .OrderBy(orderBy)
                .ToListAsync();
        }
        #endregion

        #region list
        public IList<T> List(SortDefinition<T> sort, FilterDefinition<T> filter)
        {
            return Collection.Find(filter).Sort(sort).ToList();
        }
        public IList<T> List<Tkey>(Expression<Func<T, Tkey>> orderBy, Expression<Func<T, bool>> filter = null)
        {
            IMongoQueryable<T> query = Collection.AsQueryable();
            if (filter != null)
                return query.Where(filter).OrderBy(orderBy).ToList(); 
            return query.OrderBy(orderBy).ToList();
        }

        public async Task<IList<T>> ListAsync<Tkey>(Expression<Func<T, Tkey>> orderBy, Expression<Func<T, bool>> filter = null)
        {
            IMongoQueryable<T> query = Collection.AsQueryable();
            if (filter != null)
                return await query.Where(filter).OrderBy(orderBy).ToListAsync();
            return await query.OrderBy(orderBy).ToListAsync();
        }

        #endregion

        #region count

        public long Count()
        {
            return Collection
                .Count(Builders<T>.Filter.Empty);
        }

        public long Count(Expression<Func<T, bool>> filter, CountOptions options = null)
        {
            return Collection
                .Count(filter, options);
        }

        public async Task<long> CountAsync()
        {
            return await Collection
                .CountAsync(Builders<T>.Filter.Empty);
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> filter, CountOptions options = null)
        {
            return await Collection
                .CountAsync(filter, options);
        }

        #endregion

        #region delete

        public bool Delete(Expression<Func<T, bool>> filter)
        {
            return Collection
                .DeleteOne(filter)
                .DeletedCount > 0;
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            DeleteResult result = await Collection.
                DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        #endregion

        #region queryAble

        public IMongoQueryable<T> Query()
        {
            return Collection
                .AsQueryable();
        }

        #endregion

        #region objectId

        public ObjectId CreateObjectId(string value)
        {
            return ObjectId.Parse(value);
        }

        #endregion

        #region Internal
        internal void BuilderSetCollectionName()
        {               
             MongoCollectionName mongoCollectionName = (MongoCollectionName)typeof(T)
                .GetTypeInfo()
                .GetCustomAttribute(typeof(MongoCollectionName));

            CollectionName = mongoCollectionName != null 
                ? mongoCollectionName.TableName 
                : typeof(T).Name.ToLower();

            mongoCollectionName = null;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Collection = null;
                    Connect = null;
                }
                disposed = true;
            }
        }   
        ~Repository()
        {
            Dispose(false);
        }
        private bool disposed = false;
        #endregion Dispose                    

    }
}