using Domain.Entities;
using Services.Services.Contract;
using System;

namespace Services.Abstractions
{
    public interface IServiceManager
    {
        IService<TEntity, TCreationDto, TUpdateDto> GetService<TEntity, TCreationDto, TUpdateDto>()
             where TEntity : Entity<Guid> 
             where TCreationDto : class
             where TUpdateDto : class;
    }
}
