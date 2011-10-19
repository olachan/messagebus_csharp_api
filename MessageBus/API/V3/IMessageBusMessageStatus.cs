namespace MessageBus.API.V3 {
    public interface IMessageBusMessageStatus {
        bool Succeeded { get; }
        string StatusCode { get; }
        string StatusMessage { get; }
        string MessageId { get;}
    }
}