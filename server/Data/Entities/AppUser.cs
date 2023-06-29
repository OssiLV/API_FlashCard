using Microsoft.AspNetCore.Identity;

namespace server.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        //public bool RememberMe { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
