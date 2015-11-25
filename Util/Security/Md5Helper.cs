using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Util.Security
{
    public class Md5Helper
    {
        public static string GetMd5(string data, Encoding encoding)
        {
            byte[] _buffer = encoding.GetBytes(data);
            return GetMd5(_buffer);
        }

        public static string GetMd5(byte[] data)
        {
            if (data == null)
                throw new Exception("The data to be encrypted could not be null.");

            MD5 _md5 = MD5CryptoServiceProvider.Create();
            return Convert.ToBase64String(_md5.ComputeHash(data));
        }
    }
}
