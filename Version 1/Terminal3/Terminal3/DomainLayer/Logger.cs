using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer
{
    public static class Logger
    {

        private static readonly log4net.ILog InfoLogger = log4net.LogManager.GetLogger("InfoLogger");
        private static readonly log4net.ILog ErrorLogger = log4net.LogManager.GetLogger("ErrorLogger");

        private static bool IsInitiated = false;

        private static void init()
        {
            log4net.Config.XmlConfigurator.Configure();
            IsInitiated = true;
        }
        
        //for action in the system
        public static void LogInfo(String msg)
        {
            if (!IsInitiated)
                init();
            InfoLogger.Info(msg);
        }

        //for error and failed action
        public static void LogError(String msg)
        {
            if (!IsInitiated)
                init();
            ErrorLogger.Error(msg);
        }
    }
}
