using Microsoft.AspNetCore.Identity;

namespace Domains
{
    public class ApplicationUserRole : IdentityUserRole<long>, IBaseDomain
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}