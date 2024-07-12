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
    public class UserService : IService<User, UserForCreationDto, UserForUpdateDto>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IRepository<User> _userRepository;
        private readonly IValidator<User> _userValidator;
        private readonly IEmailService _emailService; // Dependency for email service
        private readonly IConfiguration _configuration; // For SMTP details



        public UserService(IRepositoryManager repositoryManager, IValidator<User> userValidator, IEmailService emailService, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _userRepository = repositoryManager.GetRepository<User>();
            _userValidator = userValidator;
            _emailService = emailService;
            _configuration = configuration;
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

        public string GenerarMensajeBienvenida(string userName, string userLastName, string usuario, string contraseña)
        {
            return $"Hola {userName} {userLastName},\n\n" +
                   "¡Bienvenido al Sistema de Gestión de la Unión Internacional de Ciclistas!\n\n" +
                   $"Tu usuario es: {usuario}\n" +
                   $"Tu contraseña es: {contraseña}\n\n" +
                   "Por favor, recuerda que esta información es privada y no debes compartirla con nadie.\n\n" +
                   "Gracias y que tengas un excelente día.";
        }

        public async Task<User> CreateAsync(UserForCreationDto userForCreationDto, CancellationToken cancellationToken = default)
        {
            User user = userForCreationDto.Adapt<User>();

            if (!_userValidator.Validate(user))
            {
                throw new ValidationException("Failed user validation");
            }

            await _userRepository.InsertAsync(user, cancellationToken);

            var fromEmail = _configuration["SMTP:Username"]; 
            var subject = "Welcome to ISUCI!";

            var body = GenerarMensajeBienvenida(user.UserName, user.UserLastName, user.UserEmail, user.UserPassword);

            await _emailService.SendEmailAsync(fromEmail, user.UserEmail, subject, body);

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
