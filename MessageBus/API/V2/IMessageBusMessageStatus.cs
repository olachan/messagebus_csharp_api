namespace MessageBus.API.V2 {
    public interface IMessageBusMessageStatus {
        bool Succeeded { get; }
        string StatusCode { get; }
        string StatusMessage { get; }
        string MessageId { get;}
    }
}