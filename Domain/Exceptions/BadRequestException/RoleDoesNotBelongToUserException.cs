using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BadRequestException
{
    public sealed class RoleDoesNotBelongToUserException : BadRequestException
    {
        public RoleDoesNotBelongToUserException(Guid roleId)
            : base($"The Shopping with the identifier {roleId} does not belong to any User")
        {
        }
    }
}
