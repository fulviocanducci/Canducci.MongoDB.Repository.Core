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
        
        public bool Edit(Expression<Func<T, bool>> where, T model)
        {
            return Collection.ReplaceOne(where, model).ModifiedCount > 0;
        }

        public async Task<bool> EditAsync(Expression<Func<T, bool>> where, T model)
        {
            ReplaceOneResult result = await Collection.ReplaceOneAsync(where, model);
            return result.ModifiedCount > 0;
        }

        public bool Edit(FilterDefinition<T> filter, T model)
        {
            return Collection.ReplaceOne(filter, model).ModifiedCount > 0;
        }

        public async Task<bool> EditAsync(FilterDefinition<T> filter, T model)
        {
            ReplaceOneResult result = await Collection.ReplaceOneAsync(filter, model);
            return result.ModifiedCount > 0;
        }

        #endregion

        #region update

        public bool Update(Expression<Func<T, bool>> where, UpdateDefinition<T> update)
        {
            return Collection.UpdateOne(where, update).ModifiedCount > 0;
        }

        public bool UpdateAll(Expression<Func<T, bool>> where, UpdateDefinition<T> update)
        {
            return Collection.UpdateMany(where, update).ModifiedCount > 0;
        }

        public async Task<bool> UpdateAsync(Expression<Func<T, bool>> where, UpdateDefinition<T> update)
        {
            UpdateResult result = await Collection.UpdateOneAsync(where, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateAllAsync(Expression<Func<T, bool>> where, UpdateDefinition<T> update)
        {
            UpdateResult result = await Collection.UpdateManyAsync(where, update);
            return result.ModifiedCount > 0;
        }

        public bool Update(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            return Collection.UpdateOne(filter, update).ModifiedCount > 0;
        }

        public bool UpdateAll(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            return Collection.UpdateMany(filter, update).ModifiedCount > 0;
        }

        public async Task<bool> UpdateAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            UpdateResult result = await Collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> UpdateAllAsync(FilterDefinition<T> filter, UpdateDefinition<T> update)
        {
            UpdateResult result = await Collection.UpdateManyAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        #endregion

        #region find

        public T Find(Expression<Func<T, bool>> where)
        {
            return Collection.Find(where).FirstOrDefault();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> where)
        {
            IAsyncCursor<T> result = await Collection.FindAsync(where);
            return result.FirstOrDefault();
        }

        public T Find(FilterDefinition<T> filter)
        {
            return Collection.Find(filter).FirstOrDefault();
        }

        public async Task<T> FindAsync(FilterDefinition<T> filter)
        {
            IAsyncCursor<T> result = await Collection.FindAsync(filter);
            return result.FirstOrDefault();
        }

        #endregion

        #region all

        public IEnumerable<T> All()
        {
            return Collection.AsQueryable().AsEnumerable();
        }

        public IEnumerable<T> All(Expression<Func<T, bool>> where)
        {
            return Collection.AsQueryable().Where(where).AsEnumerable();
        }

        public IEnumerable<T> All<Tkey>(Expression<Func<T, bool>> where, Expression<Func<T, Tkey>> orderBy)
        {
            return Collection.AsQueryable().Where(where).OrderBy(orderBy).AsEnumerable();
        }

        public async Task<IList<T>> AllAsync()
        {
            return await Collection.AsQueryable().ToListAsync();
        }

        public async Task<IList<T>> AllAsync(Expression<Func<T, bool>> where)
        {
            return await Collection.AsQueryable().Where(where).ToListAsync();
        }

        public async Task<IList<T>> AllAsync<Tkey>(Expression<Func<T, bool>> where, Expression<Func<T, Tkey>> orderBy)
        {               
            return await Collection.AsQueryable().Where(where).OrderBy(orderBy).ToListAsync();
        }

        public IEnumerable<T> All(FilterDefinition<T> filter)
        {
            return Collection.Find(filter).ToEnumerable();
        }

        public IEnumerable<T> All(FilterDefinition<T> filter, SortDefinition<T> sort)
        {
            return Collection.Find(filter).Sort(sort).ToEnumerable();
        }

        public async Task<IList<T>> AllAsync(FilterDefinition<T> filter)
        {
            return await Collection.Find(filter).ToListAsync();
        }

        public async Task<IList<T>> AllAsync(FilterDefinition<T> filter, SortDefinition<T> sort)
        {
            return await Collection.Find(filter).Sort(sort).ToListAsync();
        }
        #endregion

        #region list

        public IList<T> List(SortDefinition<T> sort)
        {
            return Collection.Find(Builders<T>.Filter.Empty).Sort(sort).ToList();
        }

        public async Task<IList<T>> ListAsync(SortDefinition<T> sort)
        {
            return await Collection.Find(Builders<T>.Filter.Empty).Sort(sort).ToListAsync();
        }

        public IList<T> List(SortDefinition<T> sort, FilterDefinition<T> filter)
        {
            return Collection.Find(filter).Sort(sort).ToList();
        }

        public async Task<IList<T>> ListAsync(SortDefinition<T> sort, FilterDefinition<T> filter)
        {
            return await Collection.Find(filter).Sort(sort).ToListAsync();
        }

        public IList<T> List<Tkey>(Expression<Func<T, Tkey>> orderBy, Expression<Func<T, bool>> where)
        {            
            return Collection.AsQueryable().Where(where).OrderBy(orderBy).ToList();             
        }

        public async Task<IList<T>> ListAsync<Tkey>(Expression<Func<T, Tkey>> orderBy, Expression<Func<T, bool>> where)
        {            
            return await Collection.AsQueryable().Where(where).OrderBy(orderBy).ToListAsync();            
        }

        public IList<T> List<Tkey>(Expression<Func<T, Tkey>> orderBy)
        {
            return Collection.AsQueryable().OrderBy(orderBy).ToList();
        }

        public async Task<IList<T>> ListAsync<Tkey>(Expression<Func<T, Tkey>> orderBy)
        {
            return await Collection.AsQueryable().OrderBy(orderBy).ToListAsync();
        }

        #endregion

        #region count

        public long Count()
        {
            return Collection.Count(Builders<T>.Filter.Empty);
        }

        public long Count(Expression<Func<T, bool>> where, CountOptions options = null)
        {
            return Collection.Count(where, options);
        }

        public long Count(FilterDefinition<T> filter, CountOptions options = null)
        {
            return Collection.Count(filter, options);
        }

        public async Task<long> CountAsync()
        {
            return await Collection.CountAsync(Builders<T>.Filter.Empty);
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> where, CountOptions options = null)
        {
            return await Collection.CountAsync(where, options);
        }

        public async Task<long> CountAsync(FilterDefinition<T> filter, CountOptions options = null)
        {
            return await Collection.CountAsync(filter, options);
        }

        #endregion

        #region delete

        public bool Delete(Expression<Func<T, bool>> where)
        {            
            return Collection.DeleteOne(where).DeletedCount > 0;
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> where)
        {
            DeleteResult result = await Collection.DeleteOneAsync(where);
            return result.DeletedCount > 0;
        }

        public bool Delete(FilterDefinition<T> filter)
        {
            return Collection.DeleteOne(filter).DeletedCount > 0;
        }

        public async Task<bool> DeleteAsync(FilterDefinition<T> filter)
        {
            DeleteResult result = await Collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }

        public bool DeleteAll(Expression<Func<T, bool>> where)
        {
            return Collection.DeleteMany(where).DeletedCount > 0;
        }

        public async Task<bool> DeleteAllAsync(Expression<Func<T, bool>> where)
        {
            DeleteResult result = await Collection.DeleteManyAsync(where);
            return result.DeletedCount > 0;
        }

        public bool DeleteAll(FilterDefinition<T> filter)
        {
            return Collection.DeleteMany(filter).DeletedCount > 0;
        }

        public async Task<bool> DeleteAllAsync(FilterDefinition<T> filter)
        {
            DeleteResult result = await Collection.DeleteManyAsync(filter);
            return result.DeletedCount > 0;
        }


        #endregion

        #region queryAble

        public IMongoQueryable<T> Query()
        {
            return Collection.AsQueryable();
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