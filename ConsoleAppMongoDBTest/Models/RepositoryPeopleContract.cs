using Canducci.MongoDB.Repository.Core;

namespace ConsoleAppMongoDBTest.Models
{
    public abstract class RepositoryPeopleContract : Repository<People>, IRepository<People>
    {
        public RepositoryPeopleContract(IConnect connect) : base(connect)
        {
        }
    }
}
