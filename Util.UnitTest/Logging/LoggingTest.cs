using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Util.Logging;

namespace Util.UnitTest.Logging
{
    [TestClass]
    public class LoggingTest
    {
        [TestMethod]
        public void TestLogInfo()
        {
            string _writeLogContent = "This is a test log"
                , _readLogContent = string.Empty
                , _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "info.log");

            //write log
            Log4netLogger.instance.LogInfo(_writeLogContent);
            Log4netLogger.instance.ShutDown();

            //read log

            string[] _allLines = File.ReadAllLines(_logFilePath);
            if (_allLines != null && _allLines.Length > 0)
            {
                _readLogContent = _allLines[_allLines.Length - 1];
                string[] _contentArray = _readLogContent.Split('-');
                if (_contentArray.Length > 1)
                    _readLogContent = _contentArray[_contentArray.Length - 1].TrimStart();
                else
                    _readLogContent = string.Empty;
            }

            //compare log
            Assert.AreEqual(_writeLogContent, _readLogContent, string.Format("The log I writed was: {0}, and the log I read was: {1}", _writeLogContent, _readLogContent));
        }
    }
}
