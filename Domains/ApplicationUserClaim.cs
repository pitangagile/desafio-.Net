using Microsoft.AspNetCore.Identity;

namespace Domains
{
    public class ApplicationUserClaim : IdentityUserClaim<long>, IBaseDomain
    {
        public virtual ApplicationUser User { get; set; }
    }
}