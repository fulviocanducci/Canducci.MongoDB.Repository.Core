using Microsoft.Extensions.Configuration;
namespace Canducci.MongoDB.Repository.Core
{
    public class Config : IConfig
    {
        public string MongoConnectionString { get; private set; }
        public string MongoDatabase { get; private set; }
        public bool AzureConnection { get; private set; } = false;
        public bool Ssl { get; private set; } = false;

        public Config(IConfiguration configuration)
        {            
            IConfigurationSection section = configuration.GetSection("MongoDB");

            if (section == null)
                throw new RepositoryException("Section MongoDB Not Found");
            
            MongoConnectionString = section["ConnectionStrings"];
            MongoDatabase = section["Database"];

            if (bool.TryParse(section["AzureConnection"], out var azureConnection))
            {
                AzureConnection = azureConnection;
            }

            if (bool.TryParse(section["Ssl"], out var ssl))
            {
                Ssl = ssl;
            }
        }

        public Config(string connectionString, string database, bool azureConnection = false, bool ssl = false)
        {
            MongoConnectionString = connectionString;
            MongoDatabase = database;
            AzureConnection = azureConnection;
            Ssl = ssl;
        }         

    }
}
