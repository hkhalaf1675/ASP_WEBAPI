using System.ComponentModel.DataAnnotations;

namespace DBAssigment.DTOs
{
    public class LoginDto
    {
        [Required, StringLength(100)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
