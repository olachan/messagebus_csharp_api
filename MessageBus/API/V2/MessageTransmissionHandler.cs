namespace MessageBus.API.V2 {
    /// <summary>
    /// Event Delegate
    /// </summary>
    /// <param name="transmissionEvent">The event data</param>
    public delegate void MessageTransmissionHandler(IMessageBusTransmissionEvent transmissionEvent);
}