namespace Canducci.MongoDB.Repository.Core
{
    public interface IConfig
    {
        string MongoConnectionString { get; }
        string MongoDatabase { get; }
        bool AzureConnection { get; }
        bool Ssl { get; }
    }
}
