using Cn.Ubingo.Security.RSA.Key;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Security
{
    public class RsaKeyPair
    {
        /*
         * for refrence: http://www.cnblogs.com/FoChen/p/4740814.html and http://www.codeproject.com/Articles/25487/Cryptographic-Interoperability-Keys
         * XML format Key: usually used for C#
         * ASN format key: usually used for Java
         * PEM format key: usually used for Php
         */

        public string PrivateKey_XML { get; private set; }
        public string PublicKey_XML { get; private set; }
        public string PrivateKey_ASN { get; private set; }
        public string PublicKey_ASN { get; private set; }
        public string PrivateKey_PEM { get; private set; }
        public string PublicKey_PEM { get; private set; }

        public RsaKeyPair()
        {
            KeyPair _keyPair = KeyGenerator.GenerateKeyPair();

            KeyPair _xmlKeyPair = _keyPair.ToXMLKeyPair();
            this.PrivateKey_XML = _xmlKeyPair.PrivateKey;
            this.PublicKey_XML = _xmlKeyPair.PublicKey;

            KeyPair _asnKeyPair = _keyPair.ToASNKeyPair();
            this.PrivateKey_ASN = _asnKeyPair.PrivateKey;
            this.PublicKey_ASN = _asnKeyPair.PublicKey;

            KeyPair _pemKeyPair = _keyPair.ToPEMKeyPair();
            this.PrivateKey_PEM = _pemKeyPair.PrivateKey;
            this.PublicKey_PEM = _pemKeyPair.PublicKey;
        }
    }
}
