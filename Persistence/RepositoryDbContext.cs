using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Persistence
{
    public sealed class RepositoryDbContext : DbContext
    {
        public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            IEnumerable<Type> entityTypes = Assembly.GetAssembly(typeof(Entity))
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Entity)));

            foreach (Type entityType in entityTypes)
            {
                MethodInfo method = typeof(ModelBuilder).GetMethod(nameof(ModelBuilder.Entity), Type.EmptyTypes)
                    ?.MakeGenericMethod(entityType);

                method?.Invoke(modelBuilder, null);
            }
        }

        public DbSet<T> GetValues<T>() where T : Entity
        {
            return Set<T>();
        }
    }
}
