using System;
using MessageBus.API;

namespace MessageBus.Impl
{
    public class ConsoleLogger : ILogger
    {
        public void log(LogLevel level, string message)
        {
            throw new NotImplementedException();
        }

        public void log(Exception exception)
        {
            throw new NotImplementedException();
        }

        public void log(LogLevel level, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void log(LogLevel level, string message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void debug(string message)
        {
           log("DEBUG", message);
        }

        public void info(string message)
        {
            log("INFO", message);
        }

        public void error(string message)
        {
            log("ERROR", message);
        }

        private void log(string level, string message)
        {
            Console.WriteLine(String.Format("{0} MessageBus {1} {2}", DateTime.Now, level, message));
        }
    }
}