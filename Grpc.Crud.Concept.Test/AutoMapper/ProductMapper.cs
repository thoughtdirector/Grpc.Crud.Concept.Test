using AutoMapper;
using Grpc.Crud.Concept.Test.Entities;
using System;

namespace Grpc.Crud.Concept.Test.AutoMapper
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDetail>().ReverseMap();
        }
    }
}
