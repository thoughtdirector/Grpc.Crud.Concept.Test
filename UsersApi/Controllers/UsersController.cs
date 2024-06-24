using Contracts.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public UsersController(IServiceManager serviceManager) => _serviceManager = serviceManager;

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _serviceManager.UserService.GetAllAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
        {
            var userDto = await _serviceManager.UserService.GetByIdAsync(userId, cancellationToken);

            return Ok(userDto);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreateUser([FromBody] UserForCreationDto userForCreationDto)
        {
            var userDto = await _serviceManager.UserService.CreateAsync(userForCreationDto);

            return CreatedAtAction(nameof(GetUserById), new { userId = userDto.Id }, userDto);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UserForUpdateDto userForUpdateDto, CancellationToken cancellationToken)
        {
            await _serviceManager.UserService.UpdateAsync(userId, userForUpdateDto, cancellationToken);
            return Ok("User Updated");
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteUser(Guid userId, CancellationToken cancellationToken)
        {
            await _serviceManager.UserService.DeleteAsync(userId, cancellationToken);
            return Ok("User Deleted");
        }
    }
}
