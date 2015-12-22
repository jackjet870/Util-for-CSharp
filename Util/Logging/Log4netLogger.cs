using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.IO;

namespace Util.Logging
{
    public class Log4netLogger
    {
        ILog log;

        public static Log4netLogger instance = new Log4netLogger();
        
        private Log4netLogger()
        {
            log = log4net.LogManager.GetLogger("Log4netLog");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4net.config");
            if (File.Exists(path))
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            }
            else
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Log4net.config");
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            }
        }

        public void LogInfo(object msg)
        {
            instance.log.Info(msg);
        }

        public void LogDebug(object msg)
        {
            instance.log.Debug(msg);
        }

        public void LogError(object obj, Exception exc)
        {
            instance.log.Error(obj, exc);
        }

        public void LogError(object obj)
        {
            instance.log.Error(obj);
        }

        public void ShutDown()
        {
            log4net.LogManager.Shutdown();
        }
    }
}
