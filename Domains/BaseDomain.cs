using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Domains
{
    public class BaseDomain : IBaseDomain
    {
        public long Id { get; set; }
    }
}
