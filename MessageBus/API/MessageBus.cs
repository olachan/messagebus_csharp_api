using MessageBus.API.V2;
using MessageBus.Impl;

namespace MessageBus.API {
    /// <summary>
    /// MessageBus Factory class for simple, (non-inversion of control) clients
    /// </summary>
    public sealed class MessageBus {

        public const int MAJOR_VERSION = 2;
        public const int MINOR_VERSION = 2;

        public static IMessageBusClient CreateClient(string apiKey) {
            return new AutoBatchingClient(apiKey, string.Format("{0}.{1}", MAJOR_VERSION, MINOR_VERSION));
        }

        public static IMessageBusClient CreateClient(string apiKey, int majorVersion) {
            return new AutoBatchingClient(apiKey, string.Format("{0}.{1}", majorVersion, MINOR_VERSION));
        }
    }
}