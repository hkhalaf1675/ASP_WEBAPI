using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBAssigment.DTOs
{
    public class RegisterDto
    {
        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required,StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
