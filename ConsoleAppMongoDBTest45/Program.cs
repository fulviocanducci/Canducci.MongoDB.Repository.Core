using Canducci.MongoDB.Repository.Core;
using ConsoleAppMongoDBTest45.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace ConsoleAppMongoDBTest45
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfig config = new Config("mongodb://localhost", "database", false, false);            
            IConnect connect = new Connect(config);
            RepositoryCarContract repCar = new RepositoryCar(connect);

            
            var result = repCar.List(Builders<Car>.Sort.Descending(x => x.Title), Builders<Car>.Filter.Regex(x => x.Title, "/O/i"));
                       

            foreach (var item in result)
                System.Console.WriteLine($"{item.Id} - {item.Title}");

            Console.ReadKey();
        }
    }
}
