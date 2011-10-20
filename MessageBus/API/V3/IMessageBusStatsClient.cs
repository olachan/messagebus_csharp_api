using System;

namespace MessageBus.API.V3 {
    public interface IMessageBusStatsClient {
        MessageBusStatsResult[] RetrieveStats(DateTime? startDate, DateTime? endDate, string tag);
        MessageBusDeliveryErrorResult[] RetrieveDeliveryErrors(DateTime? startDate, DateTime? endDate);
        MessageBusUnsubscribeResult[] RetrieveUnsubscribes(DateTime? startDate, DateTime? endDate);
    }
}