using MongoDB.Driver;
using System;
namespace Canducci.MongoDB.Repository.Core
{
    public interface IConnect : IDisposable
    {
        IMongoClient Client { get; }

        IMongoDatabase DataBase { get; }

        MongoClientSettings Settings { get; }

        IConfig Config { get; }

        IMongoCollection<T> Collection<T>(string CollectionName);

        IMongoCollection<T> Collection<T>(string collectionName, MongoCollectionSettings settings);
    }
}
