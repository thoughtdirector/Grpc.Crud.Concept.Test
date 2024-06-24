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
    internal sealed class UserRepository : IRepository<User>
    {
        private readonly RepositoryDbContext _dbContext;
        public UserRepository(RepositoryDbContext dbContext) => _dbContext = dbContext;
        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken = default) =>
            await _dbContext.Users.Include(x => x.roles).ToListAsync(cancellationToken);
        public async Task<IEnumerable<User>> GetByRoleId(Guid roleId, CancellationToken cancellationToken = default) =>
           await _dbContext.Users.Include(x => x.roles).Where(x => x.roles.Count(x => x.Id== roleId) >= 1).ToListAsync(cancellationToken);
        public async Task<User> GetById(Guid userId, CancellationToken cancellationToken = default) =>
            await _dbContext.Users.Include(x => x.roles).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        public async Task InsertAsync(User user, CancellationToken cancellationToken = default) => await _dbContext.Users.AddAsync(user);
        public void Delete(User user) => _dbContext.Users.Remove(user);
    }
}
