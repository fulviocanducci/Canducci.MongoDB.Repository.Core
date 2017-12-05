#if (NETSTANDARD1_6 || NETSTANDARD2_0)
using Microsoft.Extensions.Configuration;
#elif (NET45)
using System.Configuration;
#endif
namespace Canducci.MongoDB.Repository.Core
{
    public class Config : IConfig
    {
        public string MongoConnectionString { get; private set; }
        public string MongoDatabase { get; private set; }
        public bool AzureConnection { get; private set; } = false;
        public bool Ssl { get; private set; } = false;

#if (NETSTANDARD1_6 || NETSTANDARD2_0)

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

#elif (NET45 || NET451 || NET452 || NET46 || NET461 || NET462 || NET47 || NET471)

        public Config()
        {            
            MongoConnectionString = ConfigurationManager.AppSettings["MongoConnectionString"];
            MongoDatabase = ConfigurationManager.AppSettings["MongoDatabase"];

            if (bool.TryParse(ConfigurationManager.AppSettings["AzureConnection"], out var azureConnection))
            {
                AzureConnection = azureConnection;
            }

            if (bool.TryParse(ConfigurationManager.AppSettings["Ssl"], out var ssl))
            {
                Ssl = ssl;
            }
        }

#endif

        public Config(string connectionString, string database, bool azureConnection = false, bool ssl = false)
        {
            MongoConnectionString = connectionString;
            MongoDatabase = database;
            AzureConnection = azureConnection;
            Ssl = ssl;
        }         

    }
}
