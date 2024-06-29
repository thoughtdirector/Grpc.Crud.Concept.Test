using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomValidations
{
    public interface IValidator<T> where T : Entity
    {
        bool Validate(T entity);
    }


}
