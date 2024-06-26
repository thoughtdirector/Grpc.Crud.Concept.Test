using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Services.Contract;

namespace Persistence.Repositories
{
    internal sealed class RoleRepository : IRepository<Shopping>
    { 

        private readonly RepositoryDbContext _dbContext;
        public RoleRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;
        public async Task<IEnumerable<Shopping>> GetAll(CancellationToken cancellationToken = default) =>
            await _dbContext.GetValues<Shopping>().Include(x => x.permissions).ToListAsync(cancellationToken);
        public async Task<Shopping> GetById(Guid roleId, CancellationToken cancellationToken = default) =>
            await _dbContext.GetValues<Shopping>().Include(x => x.permissions).FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);
        public async Task InsertAsync(Shopping role, CancellationToken cancellationToken = default) => await _dbContext.GetValues<Shopping>().AddAsync(role, cancellationToken);
        public void Delete(Shopping role) => _dbContext.GetValues<Shopping>().Remove(role);
    
    }
}