using Microsoft.EntityFrameworkCore;
using Grpc.Crud.Concept.Test.Entities;
using Microsoft.Extensions.Configuration;

namespace GrpcServiceCrud.Data
{
    public class DbContextClass : DbContext
    {
        public DbSet<Product> Product { get; set; }

        public DbContextClass(DbContextOptions<DbContextClass> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);
            });
        }
    }
}