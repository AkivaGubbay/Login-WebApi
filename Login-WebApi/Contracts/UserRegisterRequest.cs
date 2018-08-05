using LoginWebApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoginWebApi.Contracts
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "User Name is required.")]
        [StringLength(50, ErrorMessage = "User Name cannot be longer than 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [StringLength(200, ErrorMessage = "Email address cannot be longer than 200 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, ErrorMessage = "Password cannot be longer than 50 characters.")]
        public string UserPassword { get; set; }
    }
}
