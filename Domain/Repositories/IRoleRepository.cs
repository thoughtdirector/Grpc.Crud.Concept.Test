using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services.Contract
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Shopping>> GetAll(CancellationToken cancellationToken = default);
        Task<Shopping> GetById(Guid roleId, CancellationToken cancellationToken = default);
        void Insert(Shopping role);
        void Remove(Shopping role);
    }
}
