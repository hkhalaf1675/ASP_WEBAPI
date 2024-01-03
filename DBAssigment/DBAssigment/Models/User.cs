using Microsoft.AspNetCore.Identity;

namespace DBAssigment.Models
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
    }
}
