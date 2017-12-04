using System;
using System.Runtime.InteropServices;
namespace Canducci.MongoDB.Repository.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    [ComVisible(true)]
    public class MongoCollectionName: Attribute
    {
        public string TableName { get; private set; }
        public MongoCollectionName(string TableName)
        {
            this.TableName = TableName;
        }
    }
}
