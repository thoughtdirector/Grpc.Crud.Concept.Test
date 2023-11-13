using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Shopping
    {
        [Key]
        public Guid ShoppingIdentifier { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        //Many To Many Relationship
        public ICollection<User> users { get; set; }
        public ICollection<Permission> permissions { get; set; }

    }
}
