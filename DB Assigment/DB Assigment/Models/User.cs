using Microsoft.AspNetCore.Identity;

namespace DB_Assigment.Models
{
    public class User:IdentityUser
    {
        // Edit:
        // -- add the navigation property RefreshTokens
        public string? FullName { get; set; }
        public string? Address { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }
    }
}
