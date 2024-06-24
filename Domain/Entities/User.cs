using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : Entity<Guid>
    {
        public string userName { get; set; }
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public string UserRole{ get; set; }
        public string PaymentMethod { get; set; }
        //Many To Many Relactionship
        public ICollection<Shopping> roles { get; set; }
    }
}
