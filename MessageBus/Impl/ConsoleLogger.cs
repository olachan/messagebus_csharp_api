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
    }
}