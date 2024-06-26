using System;
using Contracts.DTO;
using Domain.Entities;
using Domain.Repositories;
using Services.Abstractions;
using Services.Services.Contract;

namespace Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IService<User, UserForCreationDto, UserForUpdateDto>> _lazyUserService;
        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _lazyUserService = new Lazy<IService<User, UserForCreationDto, UserForUpdateDto>>(() => new UserService(repositoryManager));
        }

       public IService<User, UserForCreationDto, UserForUpdateDto> UserService => _lazyUserService.Value;

        public IService<TEntity, TCreationDto, TUpdateDto> GetService<TEntity, TCreationDto, TUpdateDto>()
            where TEntity : Entity<Guid>
            where TCreationDto : class
            where TUpdateDto : class
        {
            throw new NotImplementedException();
        }
    }
}