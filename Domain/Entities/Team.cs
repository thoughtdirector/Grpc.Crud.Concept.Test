using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Team : Entity
    {
        public IEnumerable<User> TeamMembers { get; set; }
    }
}
