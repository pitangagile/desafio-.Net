using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domains
{
    public class ApplicationRole : IdentityRole<long>, IBaseDomain
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}
