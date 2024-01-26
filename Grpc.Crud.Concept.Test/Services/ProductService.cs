using AutoMapper;
using Grpc.Core;
using Grpc.Crud.Concept.Test.Entities;
using Grpc.Crud.Concept.Test.Repositories;

namespace Grpc.Crud.Concept.Test.Services
{
    public class ProductService : ProductServiceProto.ProductServiceProtoBase
    {
        private readonly IProductService _prductProductService;
        private readonly IMapper _mapper;
        public ProductService(IProductService prductProductService, IMapper mapper)
        {
            _prductProductService = prductProductService;
            _mapper = mapper;
        }
        public async override Task<Products> GetProductList(Empty request, ServerCallContext context)
        {
            var ProductsData = await _prductProductService.GetProductListAsync();
            Products response = new Products();
            foreach (Product Product in ProductsData)
            {
                response.Items.Add(_mapper.Map<ProductDetail>(Product));
            }
            return response;
        }
        public async override Task<ProductDetail> GetProduct(GetProductDetailRequest request, ServerCallContext context)
        {
            var Product = await _prductProductService.GetProductByIdAsync(request.ProductId);
            var ProductDetail = _mapper.Map<ProductDetail>(Product);
            return ProductDetail;
        }
        public async override Task<ProductDetail> CreateProduct(CreateProductDetailRequest request, ServerCallContext context)
        {
            var Product = _mapper.Map<Product>(request.Product);
            await _prductProductService.AddProductAsync(Product);
            var ProductDetail = _mapper.Map<ProductDetail>(Product);
            return ProductDetail;
        }
        public async override Task<ProductDetail> UpdateProduct(UpdateProductDetailRequest request, ServerCallContext context)
        {
            var Product = _mapper.Map<Product>(request.Product);
            await _prductProductService.UpdateProductAsync(Product);
            var ProductDetail = _mapper.Map<ProductDetail>(Product);
            return ProductDetail;
        }
        public async override Task<DeleteProductDetailResponse> DeleteProduct(DeleteProductDetailRequest request, ServerCallContext context)
        {
            var isDeleted = await _prductProductService.DeleteProductAsync(request.ProductId);
            var response = new DeleteProductDetailResponse
            {
                IsDelete = isDeleted
            };
            return response;
        }
    }
}