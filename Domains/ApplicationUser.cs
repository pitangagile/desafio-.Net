using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domains
{
    public class ApplicationUser : IdentityUser<long>, IBaseDomain
    {
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual DateTime LastLogin { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
