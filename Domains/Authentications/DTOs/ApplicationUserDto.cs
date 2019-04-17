using System;
using System.Collections.Generic;
using System.Text;

namespace Domains
{
    public class ApplicationUserDto: BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<PhoneDto> Phones { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime LastLogin { get; set; }
    }
}
