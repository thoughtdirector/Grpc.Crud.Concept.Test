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
    [Route("api/Teams")]
    public class TeamControllers : CrudController<Team, Team, Team>
    {
        private readonly TeamService _teamService; 
        public TeamControllers(IService<Team, Team, Team> service, TeamService teamService) : base(service)
        {
            _teamService = teamService;
        }
        [EnableCors("AllowOrigin")]
        [HttpGet("getUserByDirectorName/{Name}")]
        public async Task<IActionResult> GetByName(string? name)
        {
            var teams = await _teamService.GetByDirectorName(name);
            return Ok(teams);
        }
    }
}
