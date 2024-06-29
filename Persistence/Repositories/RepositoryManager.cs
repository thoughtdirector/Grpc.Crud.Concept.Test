using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Messages;
using Services.Services.Contract;

namespace Persistence.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public RepositoryManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IRepository<T> GetRepository<T>() where T : Entity
        {
            Type repositoryType = typeof(IRepository<T>);
            if (!_repositories.ContainsKey(repositoryType))
            {
                InitializeRepository<T>();
            }

            if (!_repositories.TryGetValue(repositoryType, out object repository))
            {
                throw new InvalidOperationException(string.Format(RepositoryMessages.RepositoryTypeNotRegistered, repositoryType.Name));
            }

            return (IRepository<T>)repository;
        }

        private void InitializeRepository<T>() where T : Entity
        {
            Type repositoryType = typeof(IRepository<T>);
            Type concreteType = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(t => !t.IsAbstract && !t.IsInterface &&
                    t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)) &&
                    typeof(IRepository<T>).IsAssignableFrom(t));

            if (concreteType != null)
            {
                object repositoryInstance = ActivatorUtilities.CreateInstance(_serviceProvider, concreteType);
                _repositories[repositoryType] = repositoryInstance;
            }
            else
            {
                throw new InvalidOperationException($"No concrete implementation found for repository of type {repositoryType.Name}.");
            }
        }
    }
}
