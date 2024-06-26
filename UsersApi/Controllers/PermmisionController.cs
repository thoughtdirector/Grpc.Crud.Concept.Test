using Contracts.DTO;
using Domain.Entities;
using Services.Abstractions;

namespace Presentation.Controllers
{
    public class PermmisionController : CrudController<Permission, PermissionForCreationDto, PermissionForUpdateDto>
    {
        public PermmisionController(IServiceManager serviceManager) : base(serviceManager)
        {
        }
    }
}
