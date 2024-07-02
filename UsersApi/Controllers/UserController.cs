using Contracts.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Services.Contract;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Cors;

namespace Presentation.Controllers
{
    [EnableCors("AllowOrigin")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : CrudController<User, UserForCreationDto, UserForUpdateDto>
    {
        private readonly UserService _userService; 
        public UsersController(IService<User, UserForCreationDto, UserForUpdateDto> service, UserService userService) : base(service)
        {
            _userService = userService;
        }
        [EnableCors("AllowOrigin")]
        [HttpGet("getUserByRole/{role}")]
        public async Task<IActionResult> GetByRole(int? role)
        {
            var users = await _userService.GetByRole(role);
            return Ok(users);
        }
    }
}
