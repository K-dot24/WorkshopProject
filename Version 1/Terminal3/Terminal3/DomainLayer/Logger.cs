using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer
{
    public class Logger
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogInfo(String msg)
        {
            log.Info(msg);
        }

        public static void LogError(String msg)
        {
            log.Error(msg);
        }

        public static void LogDebug(String msg)
        {
            log.Debug(msg);
        }
    }
}
