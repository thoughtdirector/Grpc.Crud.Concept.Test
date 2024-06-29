using Contracts.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Services.Contract;
using System.Threading.Tasks;
using System;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : CrudController<User, UserForCreationDto, UserForUpdateDto>
    {
        private readonly UserService _userService; 
        public UsersController(IService<User, UserForCreationDto, UserForUpdateDto> service, UserService userService) : base(service)
        {
            _userService = userService;
        }

        [HttpGet("getUserByRole/{role}")]
        public async Task<IActionResult> GetByRole(int? role)
        {
            var users = await _userService.GetByRole(role);
            return Ok(users);
        }
    }
}
