using MessageBus.API;

namespace MessageBus.Impl {
    public class NullLogger : ILogger {
        public void info(string message) {
        }

        public void error(string message) {
        }
    }
}