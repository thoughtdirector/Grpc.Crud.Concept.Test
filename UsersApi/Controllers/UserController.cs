using Contracts.DTO;
using Domain.Entities;
using Services.Abstractions;

namespace Presentation.Controllers
{
    public class UsersController : CrudController<User, UserForCreationDto, UserForUpdateDto>
    {
        public UsersController(IServiceManager serviceManager) : base(serviceManager)
        {
        }
    }
}
