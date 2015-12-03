using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Util.Security;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace Util.UnitTest
{
    [TestClass]
    public class SecurityTest
    {
        [TestMethod]
        public void TestRsaEncryptAndDecrypt()
        {
            string _data = "我是中国人"
                , _dataEncrypted = ""
                , _dataDecrypted = "";
            RsaKeyPair _rsaKeyPair = new RsaKeyPair();
            RsaKeyFormat _rsaKeyFormat = RsaKeyFormat.PEM;

            _dataEncrypted = RsaHelper.Encrypt(_rsaKeyFormat, _rsaKeyPair.PublicKey_PEM, _data);
            _dataDecrypted = RsaHelper.Decrypt(_rsaKeyFormat, _rsaKeyPair.PrivateKey_PEM, _dataEncrypted);

            Assert.AreEqual(_data, _dataDecrypted, string.Format("Data = {0}, DataEncrypted = {1}, DataDecrypted = {2}", _data, _dataEncrypted, _dataDecrypted));
        }

        [TestMethod]
        public void TestMd5Encrypt()
        {
            string _data = "我是陈永彬"
                , _dataEncrypted1 = "", _dataEncrypted2 = ""
                , _targetFileName = "TestMd5TragetFile.txt"
                , _targetFileContent = "this is a test content";

            //test no = 1
            _dataEncrypted1 = Md5Helper.GetMd5(_data, Encoding.ASCII);
            _dataEncrypted2 = Md5Helper.GetMd5(_data, Encoding.ASCII);
            Assert.AreEqual(_dataEncrypted1, _dataEncrypted2, string.Format("TestNo = 1, DataEncrypted1 = {0}, DataEncrypted2 = {1}", _dataEncrypted1, _dataEncrypted2));

            //test no = 2
            _dataEncrypted1 = Md5Helper.GetMd5(_data, Encoding.ASCII);
            _dataEncrypted2 = Md5Helper.GetMd5(_data + "00", Encoding.ASCII);
            Assert.AreNotEqual(_dataEncrypted1, _dataEncrypted2, string.Format("TestNo = 2, DataEncrypted1 = {0}, DataEncrypted2 = {1}", _dataEncrypted1, _dataEncrypted2));

            string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _targetFileName);

            //test no = 3
            File.WriteAllText(_filePath, _targetFileContent);
            _dataEncrypted1 = Md5Helper.GetMd5(File.ReadAllBytes(_filePath));
            _dataEncrypted2 = Md5Helper.GetMd5(File.ReadAllBytes(_filePath));
            Assert.AreEqual(_dataEncrypted1, _dataEncrypted2, string.Format("TestNo = 3, DataEncrypted1 = {0}, DataEncrypted2 = {1}", _dataEncrypted1, _dataEncrypted2));

            //test no = 4
            File.WriteAllText(_filePath, _targetFileContent);
            _dataEncrypted1 = Md5Helper.GetMd5(File.ReadAllBytes(_filePath));
            File.WriteAllText(_filePath, _targetFileContent + "\r\nanother paragrah");
            _dataEncrypted2 = Md5Helper.GetMd5(File.ReadAllBytes(_filePath));
            Assert.AreNotEqual(_dataEncrypted1, _dataEncrypted2, string.Format("TestNo = 4, DataEncrypted1 = {0}, DataEncrypted2 = {1}", _dataEncrypted1, _dataEncrypted2));
        }
    }
}
