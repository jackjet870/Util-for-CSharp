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
    public class RsaHelper
    {
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
