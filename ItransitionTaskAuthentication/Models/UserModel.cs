using System.ComponentModel.DataAnnotations;


namespace ItransitionTaskAuthentication.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }
        [Required]
        public DateTime LastLoginDate { get; set; }
        [Required]
        public bool IsBlocked { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    }
}
