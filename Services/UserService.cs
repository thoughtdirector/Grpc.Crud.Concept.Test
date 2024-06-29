using Contracts.DTO;
using CustomValidations;
using Domain.Entities;
using Domain.Exceptions.NotFoundException;
using Domain.Repositories;
using Mapster;
using Services.Services.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IService<User, UserForCreationDto, UserForUpdateDto>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IRepository<User> _userRepository;
        private readonly IValidator<User> _userValidator;

        public UserService(IRepositoryManager repositoryManager, IValidator<User> userValidator)
        {
            _repositoryManager = repositoryManager;
            _userRepository = repositoryManager.GetRepository<User>();
            _userValidator = userValidator;
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
            User user = userForCreationDto.Adapt<User>();

            if (!_userValidator.Validate(user))
            {
                throw new ValidationException("Failed user validation");
            }

            await _userRepository.InsertAsync(user, cancellationToken);

            return user;
        }

        public async Task UpdateAsync(Guid userId, UserForUpdateDto userForUpdateDto, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetById(userId, cancellationToken);
            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            user = userForUpdateDto.Adapt(user);

            if (!_userValidator.Validate(user))
            {
                throw new ValidationException("Failed user validation after update");
            }

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetByRole(int? role)
        {
            var users = await _userRepository.GetAll();
            return users.Where(user => (int?)user.UserRole == role);
        }

        public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetById(userId, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            _userRepository.Delete(user);
        }
    }
}
