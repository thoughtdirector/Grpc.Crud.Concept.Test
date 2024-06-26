using Contracts.DTO;
using Domain.Entities;
using Services.Abstractions;

namespace Presentation.Controllers
{
    public class UsersController : DynamicCrudController<User, UserForCreationDto, UserForUpdateDto>
    {
        public UsersController(IServiceManager serviceManager) : base(serviceManager)
        {
        }
    }
}
