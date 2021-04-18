using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal3.DomainLayer
{
    public class Logger
    {

        private static readonly log4net.ILog InfoLogger = log4net.LogManager.GetLogger("Info");
        private static readonly log4net.ILog ErrorLogger = log4net.LogManager.GetLogger("Error");

        public static void LogInfo(String msg)
        {
            InfoLogger.Info(msg);
        }

        public static void LogError(String msg)
        {
            ErrorLogger.Error(msg);
        }
    }
}
