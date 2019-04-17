using Microsoft.AspNetCore.Identity;

namespace Domains
{
    public class ApplicationUserToken : IdentityUserToken<long>, IBaseDomain
    {
        public virtual ApplicationUser User { get; set; }
    }
}