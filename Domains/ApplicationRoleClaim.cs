using Microsoft.AspNetCore.Identity;

namespace Domains
{
    public class ApplicationRoleClaim : IdentityRoleClaim<long>, IBaseDomain
    {
        public virtual ApplicationRole Role { get; set; }
    }
}