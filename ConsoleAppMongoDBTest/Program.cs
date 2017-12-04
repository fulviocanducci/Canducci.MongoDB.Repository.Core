using Canducci.MongoDB.Repository.Core;
using ConsoleAppMongoDBTest.Models;
using System;

namespace ConsoleAppMongoDBTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfig config = new Config("mongodb://localhost", "dabase", false, false);
            IConnect connect = new Connect(config);
            RepositoryPeopleContract repPeople = new RepositoryPeople(connect);

            People people = new People();
            people.Name = "Fúlvio";

            repPeople.Add(people);


            Console.WriteLine("Hello World!");
        }
    }
}
