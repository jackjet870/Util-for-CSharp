using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cn.Ubingo.Security.RSA.Core;
using Cn.Ubingo.Security.RSA.Key;
using Cn.Ubingo.Security.RSA.Data;

namespace Util.Security
{
    /// <summary>
    /// generate rsa key with xml, asn and pem format; 
    /// encrypt and decrypt cryptograph with key provided;
    /// </summary>
    public class RsaHelper
    {
        /*
         * for refrence: http://www.cnblogs.com/FoChen/p/4740814.html and http://www.codeproject.com/Articles/25487/Cryptographic-Interoperability-Keys
         * XML format Key: usually used for C#
         * ASN format key: usually used for Java
         * PEM format key: usually used for Php
         */

        public static RsaKeyPair GetRsaKey(RsaKeyFormat rsaKeyFormat)
        {
            KeyPair _keyPair;

            switch (rsaKeyFormat)
            {
                case RsaKeyFormat.XML:
                    _keyPair = KeyGenerator.GenerateKeyPair(KeyFormat.XML);
                    break;
                case RsaKeyFormat.ASN:
                    _keyPair = KeyGenerator.GenerateKeyPair(KeyFormat.ASN);
                    break;
                case RsaKeyFormat.PEM:
                    _keyPair = KeyGenerator.GenerateKeyPair(KeyFormat.PEM);
                    break;
                default:
                    throw new Exception(string.Format("the rsa key format: {0} is not supported.", rsaKeyFormat));
            }

            return new RsaKeyPair
            {
                PrivateKey = _keyPair.PrivateKey,
                PublicKey = _keyPair.PublicKey,
            };
        }

        public static string Encrypt(RsaKeyFormat rsaKeyFormat, string publicKey, string data)
        {
            KeyWorker _keyWorker;

            switch (rsaKeyFormat)
            { 
                case RsaKeyFormat.XML:
                    _keyWorker = new KeyWorker(publicKey, KeyFormat.XML);
                    break;
                case RsaKeyFormat.ASN:
                    _keyWorker = new KeyWorker(publicKey, KeyFormat.ASN);
                    break;
                case RsaKeyFormat.PEM:
                    _keyWorker = new KeyWorker(publicKey, KeyFormat.PEM);
                    break;
                default:
                    throw new Exception(string.Format("the rsa key format: {0} is not supported.", rsaKeyFormat));
            }
            
            return _keyWorker.Encrypt(data);
        }

        public static string Decrypt(RsaKeyFormat rsaKeyFormat, string privateKey, string data)
        {
            KeyWorker _keyWorker;

            switch (rsaKeyFormat)
            {
                case RsaKeyFormat.XML:
                    _keyWorker = new KeyWorker(privateKey, KeyFormat.XML);
                    break;
                case RsaKeyFormat.ASN:
                    _keyWorker = new KeyWorker(privateKey, KeyFormat.ASN);
                    break;
                case RsaKeyFormat.PEM:
                    _keyWorker = new KeyWorker(privateKey, KeyFormat.PEM);
                    break;
                default:
                    throw new Exception(string.Format("the rsa key format: {0} is not supported.", rsaKeyFormat));
            }

            return _keyWorker.Decrypt(data);
        }
    }
}
