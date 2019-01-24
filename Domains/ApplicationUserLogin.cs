using Microsoft.AspNetCore.Identity;

namespace Domains
{
    public class ApplicationUserLogin : IdentityUserLogin<long>, IBaseDomain
    {
        public virtual ApplicationUser User { get; set; }
    }
}