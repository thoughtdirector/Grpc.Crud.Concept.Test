using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services.Contract
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken = default);
        Task<TEntity> GetById(Guid id, CancellationToken cancellationToken = default);
        Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Delete(TEntity entity);
    }
}
