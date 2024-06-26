using System;
using System.Linq.Expressions;
using System.Reflection;
using Contracts.DTO;
using Domain.Entities;
using Domain.Repositories;
using Services.Abstractions;
using Services.Services.Contract;

namespace Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly MethodInfo _createLazyMethod;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
            _createLazyMethod = typeof(ServiceManager).GetMethod(nameof(CreateLazyService), BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public IService<TEntity, TCreationDto, TUpdateDto> GetService<TEntity, TCreationDto, TUpdateDto>()
            where TEntity : Entity<Guid>
            where TCreationDto : class
            where TUpdateDto : class
        {
            object lazyService = _createLazyMethod
                .MakeGenericMethod(typeof(TEntity), typeof(TCreationDto), typeof(TUpdateDto))
                .Invoke(this, null);

          

            object service = ((Lazy<object>)lazyService).Value;

            return (IService<TEntity, TCreationDto, TUpdateDto>)service;
        }

        private Lazy<object> CreateLazyService<TEntity, TCreationDto, TUpdateDto>()
            where TEntity : Entity<Guid>
            where TCreationDto : class
            where TUpdateDto : class
        {
            Type serviceType = typeof(IService<,,>).MakeGenericType(typeof(TEntity), typeof(TCreationDto), typeof(TUpdateDto));
            Type lazyServiceType = typeof(Lazy<>).MakeGenericType(serviceType);

            ConstantExpression repositoryManagerParameter = Expression.Constant(_repositoryManager);
            ConstructorInfo constructorInfo = serviceType.GetConstructor(new[] { typeof(IRepositoryManager) });

            if (constructorInfo == null)
            {
                throw new InvalidOperationException($"No constructor found for {serviceType.Name} that accepts IRepositoryManager parameter.");
            }

            Expression<Func<object>> lambda = Expression.Lambda<Func<object>>(
                Expression.New(
                    lazyServiceType.GetConstructor(new[] { typeof(Func<>).MakeGenericType(serviceType) }),
                    Expression.Lambda(
                        serviceType,
                        Expression.New(constructorInfo, repositoryManagerParameter)
                    )
                )
            );

            Func<object> lazyService = lambda.Compile();
            return new Lazy<object>(lazyService);
        }
    }
}
