using Grpc.Crud.Concept.Test.Entities;

namespace Grpc.Crud.Concept.Test.Repositories
{
    public interface IProductService
    {
        public Task<List<Product>> GetProductListAsync();
        public Task<Product> GetProductByIdAsync(string Id);
        public Task<Product> AddProductAsync(Product Product);
        public Task<Product> UpdateProductAsync(Product Product);
        public Task<bool> DeleteProductAsync(string Id);
    }
}

