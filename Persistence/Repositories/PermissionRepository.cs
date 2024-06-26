using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Services.Contract;

namespace Persistence.Repositories
{
    internal sealed class PermissionRepository : IRepository<Permission>
    {
        private readonly RepositoryDbContext _dbContext;
        public PermissionRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;
        public async Task<IEnumerable<Permission>> GetAll(CancellationToken cancellationToken = default) =>
            await _dbContext.GetValues<Permission>().ToListAsync(cancellationToken);
        public async Task<Permission> GetById(Guid permissionId, CancellationToken cancellationToken = default) =>
            await _dbContext.GetValues<Permission>().Include(x => x.roles).FirstOrDefaultAsync(x => x.Id == permissionId, cancellationToken);
        public async Task InsertAsync(Permission permission, CancellationToken cancellationToken = default) => await _dbContext.GetValues<Permission>().AddAsync(permission, cancellationToken);
        public void Delete(Permission permisson) => _dbContext.GetValues<Permission>().Remove(permisson);
    }
}
