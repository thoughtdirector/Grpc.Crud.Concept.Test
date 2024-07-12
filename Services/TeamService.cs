using Contracts.DTO;
using CustomValidations;
using Domain.Entities;
using Domain.Exceptions.NotFoundException;
using Domain.Repositories;
using Humanizer.Configuration;
using Mapster;
using Microsoft.Extensions.Configuration;
using Services.Services.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class TeamService : IService<Team, Team, Team>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IRepository<Team> _teamRepository;
        private readonly IEmailService _emailService; // Dependency for email service
        private readonly IConfiguration _configuration; // For SMTP details



        public TeamService(IRepositoryManager repositoryManager, IEmailService emailService, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _teamRepository = repositoryManager.GetRepository<Team>();
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Team>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var teams = await _teamRepository.GetAll(cancellationToken);
            return teams;
        }

        public async Task<Team> GetByIdAsync(Guid teamId, CancellationToken cancellationToken = default)
        {
            var team = await _teamRepository.GetById(teamId, cancellationToken);
            if (team is null)
            {
                throw new Exception("not found");
            }
            return team;
        }
        public async Task<Team> CreateAsync(Team teamForCreationDto, CancellationToken cancellationToken = default)
        {
            Team team = teamForCreationDto.Adapt<Team>();
            await _teamRepository.InsertAsync(team, cancellationToken);
            return team;
        }

        public async Task UpdateAsync(Guid teamId, Team teamForUpdateDto, CancellationToken cancellationToken = default)
        {
            var team = await _teamRepository.GetById(teamId, cancellationToken);
            if (team is null)
            {
                throw new Exception("not found");
            }

            team = teamForUpdateDto.Adapt(team);
            await _teamRepository.UpdateAsync(team, cancellationToken);
        }

        public async Task<IEnumerable<Team>> GetByDirectorName(string? directorName)
        {
            var teams = await _teamRepository.GetAll();
            return teams.Where(equipo => equipo.TeamMembers.Any(miembro =>
            miembro.UserRole == Domain.Enums.UserRoleEnum.DirectorDeportivo &&
            (miembro.UserName == directorName || miembro.UserLastName == directorName) ));
        }

        public async Task DeleteAsync(Guid teamId, CancellationToken cancellationToken = default)
        {
            var team = await _teamRepository.GetById(teamId, cancellationToken);

            if (team is null)
            {
                throw new Exception("not found");
            }

            _teamRepository.Delete(team);
        }
    }
}
