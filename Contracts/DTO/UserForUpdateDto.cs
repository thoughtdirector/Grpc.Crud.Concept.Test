using System;
using System.ComponentModel.DataAnnotations;

namespace Contracts.DTO
{
    public class UserForUpdateDto
    {
        public string? UserName { get; set; }

        [EmailAddress]
        public string? UserEmail { get; set; }

        public string? UserPassword { get; set; }

        public int? UserRole { get; set; } // Use nullable UserRole to allow updating to null

        // Cyclist Attributes
        public string? Gender { get; set; }
        public string? Speciality { get; set; }
        public string? Contextura { get; set; }
        public string? AccumulatedTime { get; set; }
        public string? EquipoId { get; set; }

        // Sports Director Attributes
        public string? Nationality { get; set; }

        // Masseur Attributes
        public string? Experience { get; set; }

        // Common properties
        public bool? IsActive { get; set; } // Use nullable bool? to allow updating to null
    }
}
