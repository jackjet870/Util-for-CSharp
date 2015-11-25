using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Security
{
    public class RsaKeyPair
    {
        /// <summary>
        /// private key, support ASN, XML and PEM format
        /// </summary>
        public string PrivateKey { get; set; }
        /// <summary>
        /// public key, support ASN, XML and PEM format
        /// </summary>
        public string PublicKey { get; set; }
    }
}
