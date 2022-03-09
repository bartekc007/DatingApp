using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Username { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }

        public static AppUser CopyFrom(MemberDto member)
        {
            var appUser = new AppUser
            {
                Id = member.Id,
                Username = member.Username,
                DateOfBirth = member.DateOfBirth,
                KnownAs = member.KnownAs,
                Gender = member.Gender,
                Introduction = member.Introduction,
                LookingFor = member.LookingFor,
                Interests = member.Interests,
                City = member.City,
                Country = member.Country
            };

            return appUser;
        }
    }
    
    
}