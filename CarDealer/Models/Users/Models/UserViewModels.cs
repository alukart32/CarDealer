using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace CarDealer.Models.Users.Models
{
    public class UserViewModels
    {
        public class CreateUserViewModel
        {
            // ...
        }
    }
    public class LoginViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}