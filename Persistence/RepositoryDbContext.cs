using Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Persistence
{
    public sealed class RepositoryDbContext
    {
        private readonly IMongoDatabase _database;

        public RepositoryDbContext(string? connectionString, string? databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>() where T : Entity
        {
            return _database.GetCollection<T>(typeof(T).Name);
        }

        public void RegisterEntities()
        {
            IEnumerable<Type> entityTypes = Assembly.GetAssembly(typeof(Entity))
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Entity)));

            foreach (Type entityType in entityTypes)
            {
                var method = typeof(RepositoryDbContext).GetMethod(nameof(GetCollection))
                    ?.MakeGenericMethod(entityType);

                method?.Invoke(this, null);
            }
        }
    }
}
