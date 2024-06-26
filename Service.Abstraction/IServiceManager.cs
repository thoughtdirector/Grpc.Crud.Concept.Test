using Domain.Entities;
using Services.Services.Contract;
using System;

namespace Services.Abstractions
{
    public interface IServiceManager
    {
        IService<TEntity, TCreationDto, TUpdateDto> GetService<TEntity, TCreationDto, TUpdateDto>()
             where TEntity : Entity 
             where TCreationDto : class
             where TUpdateDto : class;
    }
}
