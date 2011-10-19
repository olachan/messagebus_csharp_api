namespace MessageBus.API.V3 {
    /// <summary>
    /// Event Delegate
    /// </summary>
    /// <param name="transmissionEvent">The event data</param>
    public delegate void MessageTransmissionHandler(IMessageBusTransmissionEvent transmissionEvent);
}