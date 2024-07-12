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
    internal sealed class TeamRepository : IRepository<Team>
    {
        private readonly IMongoCollection<Team> _teamsCollection;

        public TeamRepository(RepositoryDbContext dbContext)
        {
            _teamsCollection = dbContext.GetCollection<Team>();
        }

        public async Task<IEnumerable<Team>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _teamsCollection.Find(_ => true).ToListAsync(cancellationToken);
        }
        public async Task<Team> GetById(Guid teamId, CancellationToken cancellationToken = default)
        {
            return await _teamsCollection.Find(x => x.Id == teamId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task InsertAsync(Team team, CancellationToken cancellationToken = default)
        {
            await _teamsCollection.InsertOneAsync(team, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(Team team, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Team>.Filter.Eq(u => u.Id, team.Id);
            await _teamsCollection.ReplaceOneAsync(filter, team, new ReplaceOptions(), cancellationToken);
        }

        public void Delete(Team team)
        {
            _teamsCollection.DeleteOne(x => x.Id == team.Id);
        }
    }
}
