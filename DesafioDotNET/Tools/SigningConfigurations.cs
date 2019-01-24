using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DesafioDotNET
{
    public class SigningConfigurations
    {
        public SecurityKey key { get; }
        public SigningCredentials signingCredentials { get; }

        public SigningConfigurations()
        {
            using(var provider = new RSACryptoServiceProvider(2048))
            {
                this.key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            this.signingCredentials = new SigningCredentials(this.key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
