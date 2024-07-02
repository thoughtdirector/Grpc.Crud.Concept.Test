using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomValidations
{
    public class UserValidator: IValidator<User>
    {
        private readonly Dictionary<UserRoleEnum, Func<User, bool>> _validations;

        public UserValidator()
        {
            _validations = new Dictionary<UserRoleEnum, Func<User, bool>>()
            {
                { UserRoleEnum.Ciclista,  user => ValidateCiclista(user) },
                { UserRoleEnum.Usuario,  user => ValidateUsuario(user) },
                { UserRoleEnum.Masajista, user => ValidateMasajista(user) },
                { UserRoleEnum.DirectorDeportivo, user => ValidateDirectorDeportivo(user) }
            };
        }

        public bool Validate(User user)
        {
            if (user.UserPassword != user.UserPasswordCofirmation)
            {
                throw new Exception("The Password and Password Confirmation are different");
            }

            if (!_validations.TryGetValue(user.UserRole, out var validationFunc))
            {
                throw new ArgumentException("Invalid UserRole provided");
            }

            return validationFunc(user);
        }

        private bool ValidateCiclista(User user)
        {
            return !string.IsNullOrWhiteSpace(user.Cedula) &&
                   !string.IsNullOrWhiteSpace(user.Gender) &&
                   !string.IsNullOrWhiteSpace(user.Speciality) &&
                   !string.IsNullOrWhiteSpace(user.Contextura) &&
                   user.Age != null && user.Height != null && user.Weight != null &&
                   user.Experience == null;
        }

        private bool ValidateUsuario(User user)
        {
            return !string.IsNullOrWhiteSpace(user.UserPassword ) &&
                   !string.IsNullOrWhiteSpace(user.UserPasswordCofirmation) &&
                   !string.IsNullOrWhiteSpace(user.UserEmail) &&
                   !string.IsNullOrWhiteSpace(user.UserName) &&
                   !string.IsNullOrWhiteSpace(user.UserLastName);
        }

        private bool ValidateMasajista(User user)
        {
            return user.Experience != null &&
                   string.IsNullOrWhiteSpace(user.Cedula) &&
                   string.IsNullOrWhiteSpace(user.Gender) &&
                   string.IsNullOrWhiteSpace(user.Speciality) &&
                   string.IsNullOrWhiteSpace(user.Contextura) &&
                   user.Age == null && user.Height == null && user.Weight == null;
        }

        private bool ValidateDirectorDeportivo(User user)
        {
            return user.Nationality != null &&
                   string.IsNullOrWhiteSpace(user.Cedula) &&
                   string.IsNullOrWhiteSpace(user.Gender) &&
                   string.IsNullOrWhiteSpace(user.Speciality) &&
                   string.IsNullOrWhiteSpace(user.Contextura) &&
                   user.Age == null && user.Height == null && user.Weight == null;
        }
    }
}
