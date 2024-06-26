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
    internal sealed class UserService : IService<User, UserForCreationDto, UserForUpdateDto>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Shopping> _shoppingRepository;

        public UserService(IRepositoryManager repositoryManager) 
        {
            _repositoryManager = repositoryManager;
            _userRepository = repositoryManager.GetRepository<User>();
            _shoppingRepository = repositoryManager.GetRepository<Shopping>();
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetAll(cancellationToken);
            return users;
        }

        public async Task<User> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetById(userId, cancellationToken);
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }
            return user;
        }

        public async Task<User> CreateAsync(UserForCreationDto userForCreationDto, CancellationToken cancellationToken = default)
        {
            var role = await _shoppingRepository.GetById(userForCreationDto.roleId, cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException(userForCreationDto.roleId);
            }
            var user = userForCreationDto.Adapt<User>();
            _userRepository.InsertAsync(user);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task UpdateAsync(Guid userId, UserForUpdateDto userForUpdateDto, CancellationToken cancellationToken = default)
        {
            User user = await _userRepository.GetById( userId, cancellationToken);
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }
            var role = await _shoppingRepository.GetById( userForUpdateDto.roleId , cancellationToken);
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
            var user = await _userRepository.GetById(userId, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            _userRepository.Delete(user);

            await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}