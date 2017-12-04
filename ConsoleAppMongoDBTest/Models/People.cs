using System;
using System.Collections.Generic;
using System.Text;
using Canducci.MongoDB.Repository.Core;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ConsoleAppMongoDBTest.Models
{
    [MongoCollectionName("peoples")]
    public class People
    {
        [BsonRequired()]
        [BsonId()]
        public ObjectId Id { get; set; }

        [BsonRequired()]
        [BsonElement("name")]
        public string Name { get; set; }
    }
}
