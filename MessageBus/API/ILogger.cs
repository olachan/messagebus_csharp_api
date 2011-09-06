using System;

namespace MessageBus.API {
    public enum LogLevel {
        Debug, Info, Warn, Error, Severe, Fatal
    }
    public interface ILogger {

        void log(LogLevel level, string message);
        void log(Exception exception);
        void log(LogLevel level, Exception exception);
        void log(LogLevel level, string message, Exception exception);

        void debug(string message);
        void info(string message);
        void error(string message);
    }
}