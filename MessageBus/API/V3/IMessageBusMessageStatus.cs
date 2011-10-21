namespace MessageBus.API.V3 {
    public interface IMessageBusMessageStatus {
        bool Succeeded { get; }
        int Status { get; }
        string MessageId { get; }
        string ToEmail { get; }
    }
}