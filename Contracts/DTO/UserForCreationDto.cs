using Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Contracts.DTO
{
    public class UserForCreationDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? UserLastName { get; set; }
        [Required]
        [EmailAddress]
        public string? UserEmail { get; set; }
        [Required]
        public string? UserPassword { get; set; }
        [Required]
        public string? UserPasswordCofirmation { get; set; }
        public string? Cedula { get; set; }

        [EnumDataType(typeof(UserRoleEnum))]
        public int? UserRole { get; set; }
        public string? Gender { get; set; }

        [EnumDataType(typeof(TipoCiclistaEnum))]
        public int? Speciality { get; set; }
        public string? Contextura { get; set; }
        public string? AccumulatedTime { get; set; }
        public string? EquipoId { get; set; }
        public DateTime? Age { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string? Nationality { get; set; }
        public DateTime? Experience { get; set; }
        public bool? IsActive { get; set; }
    }

}
