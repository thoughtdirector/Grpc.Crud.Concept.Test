using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class Entity<TId> where TId : IComparable, IComparable<TId>
    {
        [Key]
        public TId? Id { get; set; }
    }
}