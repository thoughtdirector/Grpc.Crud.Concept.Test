using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Contract;

namespace Presentation.Controllers
{
    [EnableCors("AllowOrigin")]
    [ApiController]
    [Route("api/[controller]")]
    public class CrudController<TEntity, TCreationDto, TUpdateDto> : ControllerBase
        where TEntity : Entity
        where TCreationDto : class
        where TUpdateDto : class
    {
        private readonly IService<TEntity, TCreationDto, TUpdateDto> _service;

        public CrudController(IService<TEntity, TCreationDto, TUpdateDto> service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }
        [EnableCors("AllowOrigin")]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var entities = await _service.GetAllAsync(cancellationToken);
            return Ok(entities);
        }
        [EnableCors("AllowOrigin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _service.GetByIdAsync(id, cancellationToken);

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }
        [EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TCreationDto dto)
        {
            var createdEntity = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(createdEntity) }, createdEntity);
        }
        [EnableCors("AllowOrigin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TUpdateDto dto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(id, dto, cancellationToken);

            return Ok("Entity Updated");
        }
        [EnableCors("AllowOrigin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(id, cancellationToken);

            return Ok("Entity Deleted");
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
