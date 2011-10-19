using System.Collections.Generic;

namespace MessageBus.API.V3 {
    public interface IMessageBusTransmissionEvent {
        int Count { get; }
        int SuccessCount { get; }
        int FailureCount { get; }
        List<IMessageBusMessageStatus> Statuses { get; }
    }
}