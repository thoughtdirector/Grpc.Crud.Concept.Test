using Contracts.DTO;
using Domain.Entities;
using Services.Abstractions;

namespace Presentation.Controllers
{
    public class RoleController : DynamicCrudController<Shopping, RoleForCreationDto, RoleForUpdateDto>
    {
        public RoleController(IServiceManager serviceManager) : base(serviceManager)
        {
        }
    }
}
