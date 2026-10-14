using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs
{
    public class UserDTO
    {

        [Required]
        public string FullName { get; set; } = null!;

        public string? username { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;

        public string Department { get; set; } = null!;

        public string password { get; set; } = null!;

    }
}
