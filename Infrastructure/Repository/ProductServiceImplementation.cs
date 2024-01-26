using Grpc.Crud.Concept.Test.Entities;
using GrpcServiceCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace Grpc.Crud.Concept.Test.Repositories
{
    public class ProductServiceImplementation : IProductService
    {
        private readonly DbContextClass _dbContext;
        public ProductServiceImplementation(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Product>> GetProductListAsync()
        {
            return await _dbContext.Product.ToListAsync();
        }
        public async Task<Product> GetProductByIdAsync(string Id)
        {
            return await _dbContext.Product.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<Product> AddProductAsync(Product Product)
        {
            var result = _dbContext.Product.Add(Product);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<Product> UpdateProductAsync(Product Product)
        {
            var result = _dbContext.Product.Update(Product);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<bool> DeleteProductAsync(string Id)
        {
            var filteredData = _dbContext.Product.Where(x => x.Id == Id).FirstOrDefault();
            var result = _dbContext.Remove(filteredData);
            await _dbContext.SaveChangesAsync();
            return result != null ? true : false;
        }
    }
}
