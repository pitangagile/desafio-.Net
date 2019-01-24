using System;
using System.Collections.Generic;
using System.Text;

namespace Mapping
{
    public class PhoneDto: BaseDto
    {
        public int Number { get; set; }
        public int Area_code { get; set; }
        public string Country_code { get; set; }
        public virtual ApplicationUserDto User { get; set; }
    }
}
