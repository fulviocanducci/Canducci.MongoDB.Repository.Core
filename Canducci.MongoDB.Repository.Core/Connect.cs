using System;
using System.Security.Authentication;
using MongoDB.Driver;

namespace Canducci.MongoDB.Repository.Core
{        
    public class Connect : IDisposable, IConnect
    {
        public IMongoClient Client { get; private set; }
        
        public IMongoDatabase DataBase { get; private set; }

        public MongoClientSettings Settings { get; private set; }

        public IConfig Config { get; private set; }

        public IMongoCollection<T> Collection<T>(string collectionName)
        {
            return DataBase.GetCollection<T>(collectionName); 
        }

        public IMongoCollection<T> Collection<T>(string collectionName, MongoCollectionSettings settings)
        {            
            return DataBase.GetCollection<T>(collectionName, settings);
        }

        public Connect(IConfig config)
        {
            Config = config;

            if (config.AzureConnection)
            {
                ConnectionSettingsAzure();
            }
            else
            {
                ConnectionSettingsDefault();
            }

            Client = new MongoClient(Settings);            
            
            DataBase = Client.GetDatabase(config.MongoDatabase);
        }

        protected void ConnectionSettingsDefault()
        {
            Settings = MongoClientSettings.FromUrl(new MongoUrl(Config.MongoConnectionString));
            Settings.UseSsl = Config.Ssl;
        }    
        
        protected void ConnectionSettingsAzure()
        {
            Settings = MongoClientSettings.FromUrl(new MongoUrl(Config.MongoConnectionString));
            Settings.UseSsl = Config.Ssl;
            Settings.SslSettings = new SslSettings();
            Settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            //MongoIdentity identity = new MongoInternalIdentity(config.MongoDatabase, userName);
            //MongoIdentityEvidence evidence = new PasswordEvidence(password);

            //settings.Credentials = new List<MongoCredential>()
            //{
            //    new MongoCredential("SCRAM-SHA-1", identity, evidence)
            //};
        }        

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
                    DataBase = null;
                    Client = null;
                }
                disposed = true;
            }
        }
        ~Connect()
        {
            Dispose(false);
        }
        private bool disposed = false;
        #endregion Dispose
    }
}
