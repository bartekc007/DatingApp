using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class LoginDto
    {   
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class RegisterDto
    {
        [Required]
        [MaxLength(40)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string KnownAs { get; set; }
        
    }
}