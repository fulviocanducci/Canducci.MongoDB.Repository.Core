using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Canducci.MongoDB.Repository.Core;
namespace ConsoleAppMongoDBTest45.Models
{
    [MongoCollectionName("cars")]
    public class Car
    {
        [BsonId()]
        public ObjectId Id { get; set; }

        [BsonRequired]
        [BsonElement("title")]
        public string Title { get; set; }
    }

    public abstract class RepositoryCarContract : Repository<Car>, IRepository<Car>
    {
        public RepositoryCarContract(IConnect connect) : base(connect)
        {
        }
    }

    public sealed class RepositoryCar : RepositoryCarContract
    {
        public RepositoryCar(IConnect connect) : base(connect)
        {
        }
    }
}
