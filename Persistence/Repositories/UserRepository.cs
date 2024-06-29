using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using MongoDB.Driver;
using Services.Services.Contract;

namespace Persistence.Repositories
{
    internal sealed class UserRepository : IRepository<User>
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserRepository(RepositoryDbContext dbContext)
        {
            _usersCollection = dbContext.GetCollection<User>();
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _usersCollection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<User>> GetByRole(int? roleId, CancellationToken cancellationToken = default)
        {
            return await _usersCollection.Find(x => (int?)x.UserRole == roleId).ToListAsync(cancellationToken);
        }

        public async Task<User> GetById(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _usersCollection.Find(x => x.Id == userId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task InsertAsync(User user, CancellationToken cancellationToken = default)
        {
            await _usersCollection.InsertOneAsync(user, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            await _usersCollection.ReplaceOneAsync(filter, user, new ReplaceOptions(), cancellationToken);
        }

        public void Delete(User user)
        {
            _usersCollection.DeleteOne(x => x.Id == user.Id);
        }
    }
}
