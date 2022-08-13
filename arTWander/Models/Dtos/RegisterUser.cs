using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace arTWander.Models.Dtos
{
    [ModelMetadataType(typeof(User))]
    public class RegisterUser
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmed { get; set; }

        public string? UserName { get; set; }

        public string? Name { get; set; }

        public DateTime? Birthday { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
