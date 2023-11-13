using Contracts.DTO;
using Domain.Entities;
using Domain.Exceptions.BadRequestException;
using Domain.Exceptions.NotFoundException;
using Domain.Repositories;
using Mapster;
using Services.Services.Contract;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    internal sealed class RoleService : IRoleService
    {
        private readonly IRepositoryManager _repositoryManager;

        public RoleService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

        public async Task<IEnumerable<Shopping>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var roles = await _repositoryManager.RoleRepository.GetAll(cancellationToken);
            return roles;
        }


        public async Task<Shopping> GetByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            var role = await _repositoryManager.RoleRepository.GetById(roleId, cancellationToken);
            if (role is null)
            {
                throw new UserNotFoundException(roleId);
            }
            return role;
        }

        public async Task<Shopping> CreateAsync(RoleForCreationDto roleForCreationDto, CancellationToken cancellationToken = default)
        {
            var permission = await _repositoryManager.RoleRepository.GetById(roleForCreationDto.permissionsId, cancellationToken);
            if (permission is null)
            {
                throw new PremissionNotFoundException(roleForCreationDto.permissionsId);
            }
            var Role = roleForCreationDto.Adapt<Shopping>();
            _repositoryManager.RoleRepository.Insert(Role);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Role;
        }

        public async Task UpdateAsync(Guid roleId, RoleForUpdateDto roleForUpdateDto, CancellationToken cancellationToken = default)
        {
            var role = await _repositoryManager.RoleRepository.GetById(roleId, cancellationToken);
            if (role is null)
            {
                throw new UserNotFoundException(roleId);
            }
            var permission = await _repositoryManager.PermissionRepository.GetById(roleForUpdateDto.permissionsId, cancellationToken);
            if (permission is null)
            {
                throw new RoleNotFoundException(roleForUpdateDto.permissionsId);
            }
            role.RoleDescription = roleForUpdateDto.RoleDescription;
            role.permissions.Add(permission);


            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            var role = await _repositoryManager.RoleRepository.GetById(roleId, cancellationToken);

            if (role is null)
            {
                throw new UserNotFoundException(roleId);
            }

            _repositoryManager.RoleRepository.Remove(role);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}