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
    internal sealed class UserService : IUserService
    {
        private readonly IRepositoryManager _repositoryManager;

        public UserService(IRepositoryManager repositoryManager) => _repositoryManager = repositoryManager;

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var users = await _repositoryManager.UserRepository.GetAll(cancellationToken);
            return users;
        }

        public async Task<IEnumerable<User>> GetByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            var role = await _repositoryManager.RoleRepository.GetById(roleId, cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException(roleId);
            }
            var users = await _repositoryManager.UserRepository.GetByRoleId(roleId, cancellationToken);
            if (users is null)
            {
                throw new RoleDoesNotBelongToUserException(roleId);
            }
            return users;
        }

        public async Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _repositoryManager.UserRepository.GetById(userId, cancellationToken);
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }
            return user;
        }

        public async Task<User> CreateAsync(UserForCreationDto userForCreationDto, CancellationToken cancellationToken = default)
        {
            var role = await _repositoryManager.RoleRepository.GetById(userForCreationDto.roleId, cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException(userForCreationDto.roleId);
            }
            var user = userForCreationDto.Adapt<User>();
            _repositoryManager.UserRepository.Insert(user);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task UpdateAsync(Guid userId, UserForUpdateDto userForUpdateDto, CancellationToken cancellationToken = default)
        {
            User user = await _repositoryManager.UserRepository.GetById( userId, cancellationToken);
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }
            var role = await _repositoryManager.RoleRepository.GetById( userForUpdateDto.roleId , cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException(userForUpdateDto.roleId);
            }
            user.userEmail = userForUpdateDto.userEmail;
            user.roles.Add(role);
            

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _repositoryManager.UserRepository.GetById(userId, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            _repositoryManager.UserRepository.Remove(user);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}