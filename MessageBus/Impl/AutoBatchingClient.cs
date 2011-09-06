using System;
using System.Collections.Generic;
using System.Net;
using MessageBus.API;
using MessageBus.API.V2;
using MessageBus.API.V2.Debug;
using MessageBus.SPI;

namespace MessageBus.Impl {

    /// <summary>
    /// An implementation of IMessageBusClient that automatically buffers and transmits email to the server in batches. 
    /// </summary>
    public class AutoBatchingClient : IMessageBusClient, IMessageBusDebugging {

        public event MessageTransmissionHandler Transmitted;

        public string ApiKey { get; private set; }

        public string ApiVersion { get; private set; }

        private ILogger Logger;
        private readonly IMessageBusHttpClient HttpClient;
        private BatchEmailRequest CurrentRequest;

        public AutoBatchingClient(string apiKey, string version)
            : this(apiKey, version, new SimpleHttpClient(), new ConsoleLogger()) {

        }

        public AutoBatchingClient(string apiKey, string version, IMessageBusHttpClient httpClient)
            : this(apiKey, version, httpClient, new ConsoleLogger()) {
        }

        public AutoBatchingClient(string apiKey, string version, IMessageBusHttpClient httpClient, ILogger logger) {
            ApiKey = apiKey;
            ApiVersion = version;
            HttpClient = httpClient;
            Logger = logger;
            EmailBufferSize = 20;
        }

        public string Domain {
            set { HttpClient.Domain = value; }
        }

        public string Path {
            set { HttpClient.Path = value; }
        }

        public bool SslVerifyPeer {
            set { HttpClient.SslVerifyPeer = value; }
        }

        public IWebProxy Proxy {
            set { HttpClient.Proxy = value; }
        }

        public ICredentials Credentials {
            set { HttpClient.Credentials = value; }
        }

        public int EmailBufferSize { get; set; }

        public bool Flush() {
            var result = false;
            lock (this) {
                if (CurrentRequest != null) {
                    HttpClient.SendEmails(CurrentRequest);
                    CurrentRequest = new BatchEmailRequest(this);
                    result = true;
                }
            }
            return result;
        }

        public bool Close() {
            return Flush();
        }

        public bool Send(MessageBusEmail email) {
            lock (this) {
                bool result = false;
                if (CurrentRequest == null) {
                    CurrentRequest = new BatchEmailRequest(this);
                }

                CurrentRequest.messages.Add(new BatchEmailMessage(email));

                if (CurrentRequest.messageCount >= EmailBufferSize) {
                    var response = HttpClient.SendEmails(CurrentRequest);
                    OnTranmission(response);
                    CurrentRequest = new BatchEmailRequest(this);
                    result = true;
                }
                return result;
            }
        }

        public void OnTranmission(BatchEmailResponse response) {
            MessageTransmissionHandler handler = Transmitted;
            if (handler != null) {
                var e = new TransmissionEvent(response);
                handler(e);
            }
        }

        public string TemplateKey { get; set; }

        public string FromEmail { get; set; }

        public string FromName { get; set; }

        public string[] Tags { get; set; }

        public Dictionary<string, string> CustomHeaders { get; private set; }

        public void Dispose() {
            Flush();
        }
    }

    /// <summary>
    /// internal implementation for event handling
    /// </summary>
    internal class TransmissionEvent : IMessageBusTransmissionEvent {
        public TransmissionEvent(BatchEmailResponse response) {
            Count = response.successCount + response.failureCount;
            SuccessCount = response.successCount;
            FailureCount = response.failureCount;
            StatusMessage = response.statusMessage;
            Statuses = new List<IMessageBusMessageStatus>(Count);
            foreach (var result in response.results) {
                Statuses.Add(new MessageStatus(result));
            }
        }

        public List<IMessageBusMessageStatus> Statuses { get; private set; }

        public string StatusMessage { get; private set; }

        public int FailureCount { get; private set; }

        public int SuccessCount { get; private set; }

        public int Count { get; private set; }
    }

    /// <summary>
    /// Internal implementation for event handling
    /// </summary>
    internal class MessageStatus : IMessageBusMessageStatus {
        public MessageStatus(BatchEmailResult response) {
            Succeeded = (response.status == "OK");
            StatusCode = response.status;
            StatusMessage = response.statusMessage;
            MessageId = response.messageId;
        }

        public bool Succeeded { get; private set; }

        public string StatusCode { get; private set; }

        public string StatusMessage { get; private set; }

        public string MessageId { get; private set; }
    }
}
