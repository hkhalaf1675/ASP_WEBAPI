using Microsoft.EntityFrameworkCore;

namespace DB_Assigment.Models
{
    [Owned]// to make it not require PK and make it belonged to another table
    // it will create an auto incremented PK
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.UtcNow > ExpiresOn;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}
