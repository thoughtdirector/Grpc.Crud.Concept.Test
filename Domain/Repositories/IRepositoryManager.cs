using Domain.Entities;
using Services.Services.Contract;
using System;

namespace Domain.Repositories
{
    public interface IRepositoryManager
    {
        IRepository<T> GetRepository<T>() where T : Entity;
    }
}
