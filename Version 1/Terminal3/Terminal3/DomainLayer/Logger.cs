using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer
{
    public class Logger
    {

        private static readonly log4net.ILog InfoLogger = log4net.LogManager.GetLogger("InfoLogger");
        private static readonly log4net.ILog ErrorLogger = log4net.LogManager.GetLogger("ErrorLogger");

        private static bool IsInitiated = false;

        private static void init()
        {
            log4net.Config.XmlConfigurator.Configure();
            IsInitiated = true;
        }
        
        public static void LogInfo(String msg)
        {
            if (!IsInitiated)
                init();
            InfoLogger.Info(msg);
        }

        public static void LogError(String msg)
        {
            if (!IsInitiated)
                init();
            ErrorLogger.Error(msg);
        }
    }
}
