using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Cache;

namespace Domain.Entities
    {
        public class User : Entity
        {
            public string? UserName { get; set; }
            public string? UserLastName { get; set; }
            public string? UserEmail { get; set; }
            public string? UserPassword { get; set; }
            public string? UserPasswordCofirmation { get; set; }

            public UserRoleEnum UserRole { get; set; }

            // Cyclist Attributes
            public string? Cedula { get; set; }
            public string? Gender { get; set; }
            public string? Speciality { get; set; }
            public string? Contextura { get; set; }
            public string? AccumulatedTime { get; set; }
            public string? EquipoId { get; set; }
            public int? Age { get; set; }
            public double? Height {  get; set; }
            public double? Weight { get; set; }

            // Sports Director Attributes
            public string? Nationality { get; set; }

            // Masseur Attributes
            public string? Experience { get; set; }

            // Common properties
            public bool? IsActive { get; set; }
        }
}
