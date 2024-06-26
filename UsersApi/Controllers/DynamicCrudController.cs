using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Services.Contract;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrudController<TEntity, TCreationDto, TUpdateDto> : ControllerBase
        where TEntity : Entity
        where TCreationDto : class
        where TUpdateDto : class
    {
        private readonly IServiceManager _serviceManager;
        private readonly Type _serviceType;

        public CrudController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
            _serviceType = typeof(IService<,,>).MakeGenericType(typeof(TEntity), typeof(TCreationDto), typeof(TUpdateDto));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var getAllAsyncMethod = GetServiceMethod(nameof(IService<TEntity, TCreationDto, TUpdateDto>.GetAllAsync));
            var entities = await (Task<IEnumerable<TEntity>>)getAllAsyncMethod.Invoke(_serviceManager.GetService<TEntity, TCreationDto, TUpdateDto>(), new object[] { cancellationToken });
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var getByIdAsyncMethod = GetServiceMethod(nameof(IService<TEntity, TCreationDto, TUpdateDto>.GetByIdAsync));
            var entity = await (Task<TEntity>)getByIdAsyncMethod.Invoke(_serviceManager.GetService<TEntity, TCreationDto, TUpdateDto>(), new object[] { id, cancellationToken });

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TCreationDto dto)
        {
            var createAsyncMethod = GetServiceMethod(nameof(IService<TEntity, TCreationDto, TUpdateDto>.CreateAsync));
            var createdEntity = await (Task<TEntity>)createAsyncMethod.Invoke(_serviceManager.GetService<TEntity, TCreationDto, TUpdateDto>(), new object[] { dto });

            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(createdEntity) }, createdEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TUpdateDto dto, CancellationToken cancellationToken)
        {
            var updateAsyncMethod = GetServiceMethod(nameof(IService<TEntity, TCreationDto, TUpdateDto>.UpdateAsync));
            await (Task)updateAsyncMethod.Invoke(_serviceManager.GetService<TEntity, TCreationDto, TUpdateDto>(), new object[] { id, dto, cancellationToken });

            return Ok("Entity Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleteAsyncMethod = GetServiceMethod(nameof(IService<TEntity, TCreationDto, TUpdateDto>.DeleteAsync));
            await (Task)deleteAsyncMethod.Invoke(_serviceManager.GetService<TEntity, TCreationDto, TUpdateDto>(), new object[] { id, cancellationToken });

            return Ok("Entity Deleted");
        }

        private MethodInfo GetServiceMethod(string methodName)
        {
            var method = _serviceType.GetMethod(methodName);
            if (method == null)
            {
                throw new InvalidOperationException($"Method {methodName} not found on {_serviceType.Name}.");
            }
            return method;
        }

        private Guid GetEntityId(TEntity entity)
        {
            var propertyInfo = typeof(TEntity).GetProperty("Id");
            if (propertyInfo == null || !(propertyInfo.GetValue(entity) is Guid entityId))
            {
                throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} does not have an 'Id' property or it is not of type Guid.");
            }
            return entityId;
        }
    }
}
