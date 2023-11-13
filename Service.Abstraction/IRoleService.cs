using Contracts.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services.Contract
{
    public interface IRoleService
    {
        Task<IEnumerable<Shopping>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Shopping> GetByIdAsync(Guid roleId, CancellationToken cancellationToken = default);

        Task<Shopping> CreateAsync(RoleForCreationDto roleForCreation, CancellationToken cancellationToken = default);

        Task UpdateAsync(Guid roleId, RoleForUpdateDto rolForUpdate, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid roleId, CancellationToken cancellationToken = default);
    }
}
