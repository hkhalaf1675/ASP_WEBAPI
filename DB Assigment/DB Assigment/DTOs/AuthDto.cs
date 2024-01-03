using System.Text.Json.Serialization;

namespace DB_Assigment.DTOs
{
    public class AuthDto
    {
        public string? Message { get; set; }
        public string? Token { get; set; }
        [JsonIgnore]
        public string? RefershToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
    }
}
