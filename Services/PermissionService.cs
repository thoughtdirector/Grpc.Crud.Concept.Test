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
    internal sealed class PermissionService : IPermissionService
    {
        private readonly IRepositoryManager _repositoryManager;


        public PermissionService(IRepositoryManager repositoryManager) 
        {
            _repositoryManager = repositoryManager;


        }

        public async Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IRepository<Permission> permissionRepository = _repositoryManager.GetRepository<Permission>();
            var permissions = await permissionRepository.GetAll(cancellationToken);
            return permissions;
        }


        public async Task<Permission> GetByIdAsync(Guid permissionId, CancellationToken cancellationToken = default)
        {
            var permission = await _repositoryManager.PermissionRepository.GetById(permissionId, cancellationToken);
            if (permission is null)
            {
                throw new PremissionNotFoundException(permissionId);
            }
            return permission;
        }

        public async Task<Permission> CreateAsync(PermissionForCreationDto permissionForCreationDto, CancellationToken cancellationToken = default)
        {
           
            var permission = permissionForCreationDto.Adapt<Permission>();
            _repositoryManager.PermissionRepository.Insert(permission);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return permission;
        }

        public async Task UpdateAsync(Guid permissionId, PermissionForUpdateDto permissionForUpdateDto, CancellationToken cancellationToken = default)
        {
            var permission = await _repositoryManager.PermissionRepository.GetById(permissionId, cancellationToken);
            if (permission is null)
            {
                throw new PremissionNotFoundException(permissionId);
            }
            permission.permissionDescription = permissionForUpdateDto.permissionDescription;


            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid permissionId, CancellationToken cancellationToken = default)
        {
            var permission = await _repositoryManager.PermissionRepository.GetById(permissionId, cancellationToken);

            if (permission is null)
            {
                throw new UserNotFoundException(permissionId);
            }

            _repositoryManager.PermissionRepository.Remove(permission);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}