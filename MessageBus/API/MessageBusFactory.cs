using MessageBus.API.V3;
using MessageBus.Impl;

namespace MessageBus.API {
    /// <summary>
    /// MessageBus Factory class for simple, (non-inversion of control) clients
    /// </summary>
    public sealed class MessageBusFactory {

        public static IMessageBusEmailClient CreateEmailClient(string apiKey) {
            return new AutoBatchingEmailClient(apiKey);
        }

        public static IMessageBusEmailClient CreateEmailClient(string apiKey, ILogger logger) {
            return new AutoBatchingEmailClient(apiKey, logger);
        }

        public static IMessageBusStatsClient CreateStatsClient(string apiKey) {
            return new DefaultStatsClient(apiKey);
        }

        public static IMessageBusStatsClient CreateStatsClient(string apiKey, ILogger logger) {
            return new DefaultStatsClient(apiKey, logger);
        }

        public static IMessageBusMailingListClient CreateMailingListClient(string apiKey) {
            return new DefaultMailingListClient(apiKey);
        }

        public static IMessageBusMailingListClient CreateMailingListClient(string apiKey, ILogger logger) {
            return new DefaultMailingListClient(apiKey, logger);
        }

    }
}