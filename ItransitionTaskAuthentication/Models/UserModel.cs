using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ItransitionTaskAuthentication.Models
{
    [Index(nameof(Id), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsBlocked { get; set; }
    }
}