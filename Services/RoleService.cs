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
    internal sealed class RoleService : IService<Shopping, RoleForCreationDto, RoleForUpdateDto>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IRepository<Shopping> _shoppingRepository;
        private readonly IRepository<Permission> _permissionRepository;


        public RoleService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
            _shoppingRepository = repositoryManager.GetRepository<Shopping>();
        }

        public async Task<IEnumerable<Shopping>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<Shopping> roles = await _shoppingRepository.GetAll(cancellationToken);
            return roles;
        }


        public async Task<Shopping> GetByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            Shopping role = await _shoppingRepository.GetById(roleId, cancellationToken);
            if (role is null)
            {
                throw new UserNotFoundException(roleId);
            }
            return role;
        }

        public async Task<Shopping> CreateAsync(RoleForCreationDto roleForCreationDto, CancellationToken cancellationToken = default)
        {
            Shopping permission = await _shoppingRepository.GetById(roleForCreationDto.permissionsId, cancellationToken);
            if (permission is null)
            {
                throw new PremissionNotFoundException(roleForCreationDto.permissionsId);
            }
            Shopping Role = roleForCreationDto.Adapt<Shopping>();
            _shoppingRepository.InsertAsync(Role);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Role;
        }

        public async Task UpdateAsync(Guid roleId, RoleForUpdateDto roleForUpdateDto, CancellationToken cancellationToken = default)
        {
            Shopping role = await _shoppingRepository.GetById(roleId, cancellationToken);
            if (role is null)
            {
                throw new UserNotFoundException(roleId);
            }
            Permission permission = await _permissionRepository.GetById(roleForUpdateDto.permissionsId, cancellationToken);
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
            Shopping role = await _shoppingRepository.GetById(roleId, cancellationToken);

            if (role is null)
            {
                throw new UserNotFoundException(roleId);
            }

            _shoppingRepository.Delete(role);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}