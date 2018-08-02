using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class UserRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be atleast 8 characters")]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

    }
}
