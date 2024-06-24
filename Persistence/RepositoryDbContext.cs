using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Persistence
{
    public sealed class RepositoryDbContext : DbContext
    {
        public RepositoryDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<T> GetValues<T>() where T : Entity<Guid> 
        {
            return (DbSet<T>)this.GetType().GetProperties().FirstOrDefault(it => typeof(DbSet<>).MakeGenericType(typeof(T)) == it.PropertyType)!.GetValue(this)!;
        }

    }
}
    