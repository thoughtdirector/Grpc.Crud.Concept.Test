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
    internal sealed class PermissionService : IService<Permission, PermissionForCreationDto, PermissionForUpdateDto>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IRepository<Permission> _permissionRepository;



        public PermissionService(IRepositoryManager repositoryManager) 
        {
            _repositoryManager = repositoryManager;
            _permissionRepository = repositoryManager.GetRepository<Permission>();

        }

        public async Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var permissions = await _permissionRepository.GetAll(cancellationToken);
            return permissions;
        }


        public async Task<Permission> GetByIdAsync(Guid permissionId, CancellationToken cancellationToken = default)
        {
            var permission = await _permissionRepository.GetById(permissionId, cancellationToken);
            if (permission is null)
            {
                throw new PremissionNotFoundException(permissionId);
            }
            return permission;
        }

        public async Task<Permission> CreateAsync(PermissionForCreationDto permissionForCreationDto, CancellationToken cancellationToken = default)
        {
           
            var permission = permissionForCreationDto.Adapt<Permission>();
            await _permissionRepository.InsertAsync(permission);
            
            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return permission;
        }

        public async Task UpdateAsync(Guid permissionId, PermissionForUpdateDto permissionForUpdateDto, CancellationToken cancellationToken = default)
        {
            var permission = await _permissionRepository.GetById(permissionId, cancellationToken);
            if (permission is null)
            {
                throw new PremissionNotFoundException(permissionId);
            }
            permission.permissionDescription = permissionForUpdateDto.permissionDescription;


            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid permissionId, CancellationToken cancellationToken = default)
        {
            var permission = await _permissionRepository.GetById(permissionId, cancellationToken);

            if (permission is null)
            {
                throw new UserNotFoundException(permissionId);
            }

            _permissionRepository.Delete(permission);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}