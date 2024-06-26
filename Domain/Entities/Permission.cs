using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Permission: Entity
    {
        public string permissionName { get; set; }
        public string permissionDescription { get; set; }

        //Many To Many Relationship
        public ICollection<Shopping> roles { get; set; }
    }
}
