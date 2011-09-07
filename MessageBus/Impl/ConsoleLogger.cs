using System;
using MessageBus.API;

namespace MessageBus.Impl
{
    public class ConsoleLogger : ILogger
    {
        public void info(string message)
        {
            Log("INFO", message);
        }

        public void error(string message)
        {
            Log("ERROR", message);
        }

        private static void Log(string level, string message)
        {
            Console.WriteLine(String.Format("{0} MessageBus {1} {2}", DateTime.Now, level, message));
        }
    }
}