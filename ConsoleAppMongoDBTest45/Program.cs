using Canducci.MongoDB.Repository.Core;
using ConsoleAppMongoDBTest45.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppMongoDBTest45
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfig config = new Config("mongodb://localhost", "database", false, false);            
            IConnect connect = new Connect(config);
            RepositoryCarContract repCar = new RepositoryCar(connect);

            var result = repCar.List(x => x.Title);

            foreach (var item in result)
                System.Console.WriteLine($"{item.Id} - {item.Title}");

            Console.WriteLine("Hello World!");
        }
    }
}
