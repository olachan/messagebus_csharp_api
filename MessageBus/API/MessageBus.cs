using MessageBus.API.V3;
using MessageBus.Impl;

namespace MessageBus.API {
    /// <summary>
    /// MessageBus Factory class for simple, (non-inversion of control) clients
    /// </summary>
    public sealed class MessageBus {

        public static IMessageBusClient CreateClient(string apiKey) {
            return new AutoBatchingClient(apiKey);
        }

        public static IMessageBusClient CreateClient(string apiKey, ILogger logger) {
            return new AutoBatchingClient(apiKey, logger);
        }
    }
}