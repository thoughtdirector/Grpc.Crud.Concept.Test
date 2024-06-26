using Contracts.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services.Contract
{
    public interface IService<TEntity, TCreationDto, TUpdateDto>
        where TEntity : Entity
        where TCreationDto : class 
        where TUpdateDto : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<TEntity> CreateAsync(TCreationDto permissionForCreation, CancellationToken cancellationToken = default);

        Task UpdateAsync(Guid id, TUpdateDto permissionForUpdate, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
