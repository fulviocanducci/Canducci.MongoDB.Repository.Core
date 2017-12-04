using Canducci.MongoDB.Repository.Core;

namespace ConsoleAppMongoDBTest.Models
{
    public sealed class RepositoryPeople : RepositoryPeopleContract
    {
        public RepositoryPeople(IConnect connect) : base(connect)
        {
        }
    }
}
