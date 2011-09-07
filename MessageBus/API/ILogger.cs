using System;

namespace MessageBus.API {

    public interface ILogger {
        void info(string message);
        void error(string message);
        void warning(string message);
    }
}