using Microsoft.AspNetCore.Identity;

namespace MagicVillaApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
