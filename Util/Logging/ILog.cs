using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Logging
{
    interface ILog
    {
        void Debug(object message);
        void Error(object message);
        void Info(object message);
    }
}
