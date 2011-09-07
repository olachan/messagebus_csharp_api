using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MessageBus.API;
using MessageBus.API.V2;
using MessageBus.API.V2.Debug;
using MessageBus.SPI;
using MessageBusTest.Impl;

namespace MessageBus.Impl {

    /// <summary>
    /// An implementation of IMessageBusClient that automatically buffers and transmits email to the server in batches. 
    /// </summary>
    public class AutoBatchingClient : IMessageBusClient, IMessageBusDebugging {

        public event MessageTransmissionHandler Transmitted;

        public string ApiKey { get; private set; }

        public string ApiVersion { get; private set; }

        private readonly ILogger Logger;
        private readonly IMessageBusHttpClient HttpClient;

        private BatchEmailRequest CurrentRequest;

        public AutoBatchingClient(string apiKey, string version)
            : this(apiKey, version, new SimpleHttpClient(), new NullLogger()) {
        }

        public AutoBatchingClient(string apiKey, string version, IMessageBusHttpClient httpClient)
            : this(apiKey, version, httpClient, new NullLogger()) {
        }

        public AutoBatchingClient(string apiKey, string version, ILogger logger)
            : this(apiKey, version, new SimpleHttpClient(logger), logger) {
        }

        public AutoBatchingClient(string apiKey, string version, IMessageBusHttpClient httpClient, ILogger logger) {
            ApiKey = apiKey;
            ApiVersion = version;
            HttpClient = httpClient;
            Logger = logger;
            EmailBufferSize = 20;
            CustomHeaders = new Dictionary<string, string>();
            Logger.info(String.Format("AutoBatchingClient created for version {0} with http client class {1}", version, httpClient.GetType().Name));
        }

        public bool SkipValidation { private get; set; }


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
            var result = 0;
            lock (this) {
                if (CurrentRequest != null) {
                    var response = HttpClient.SendEmails(CurrentRequest);
                    OnTranmission(response);
                    result = CurrentRequest.messageCount;
                    CurrentRequest = new BatchEmailRequest(this);
                }
            }
            Logger.info(String.Format("Flush Complete: {0} messages tranmitted.", result));
            return result > 0;
        }

        public bool Close() {
            return Flush();
        }

        public bool Send(MessageBusEmail email) {
            Validate(email);
            return Send(new BatchEmailMessage(email));
        }

        public bool Send(MessageBusTemplateEmail email) {
            Validate(email);
            return Send(new BatchEmailMessage(email));
        }

        private bool Send(BatchEmailMessage email) {

            lock (this) {
                var result = 0;
                if (CurrentRequest == null) {
                    CurrentRequest = new BatchEmailRequest(this);
                }

                CurrentRequest.messages.Add(email);

                if (CurrentRequest.messageCount >= EmailBufferSize) {
                    var response = HttpClient.SendEmails(CurrentRequest);
                    OnTranmission(response);
                    result = CurrentRequest.messageCount;
                    CurrentRequest = new BatchEmailRequest(this);
                }
                Logger.info(String.Format("Send Complete: {0} messages buffered and {1} messages tranmitted.", 1, result));
                return result > 0;
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

        private void Validate(MessageBusTemplateEmail email) {
            if (SkipValidation) return;
            string msg = "";

            if (String.IsNullOrEmpty(ApiKey)) {
                msg = "ApiKey is required";
            }
            if (String.IsNullOrEmpty(ApiVersion)) {
                msg = "ApiVersion is required";
            }

            if (String.IsNullOrEmpty(TemplateKey)) {
                msg = "A TemplateKey must be supplied when sending templated email";
            }

            if (!email.MergeFields.ContainsKey("%EMAIL%")) {
                msg = "The %EMAIL% key is required";
            }

            if (email.MergeFields.Count > 0) {
                if (email.MergeFields.Any(pair => !pair.Key.StartsWith("%") || !pair.Key.EndsWith("%"))) {
                    msg = "Merge Fields must be surrounded with %% e.g. %FIELD%";
                }
            }

            if (CustomHeaders.ContainsKey("message-id")) {
                msg = "The message-id header is reserved for internal use";
            }

            if (msg.Length > 0) {
                Logger.error(msg);
                throw new MessageBusValidationFailedException(msg);
            }

            if (!String.IsNullOrEmpty(FromEmail)) {
                Logger.warning("'FromEmail' is ignored when sending template email");
            }

            if (!String.IsNullOrEmpty(FromName)) {
                Logger.warning("'FromName' is ignored when sending template email");
            }

            if (Tags != null && Tags.Length > 0) {
                Logger.warning("Tags are ignored when sending template email");
            }
        }

        private void Validate(MessageBusEmail email) {
            if (SkipValidation) return;
            string msg = "";

            if (String.IsNullOrEmpty(ApiKey)) {
                msg = "ApiKey is required";
            }
            if (String.IsNullOrEmpty(ApiVersion)) {
                msg = "ApiVersion is required";
            }

            if (String.IsNullOrEmpty(FromEmail)) {
                msg = "From Email is required";
            }

            if (String.IsNullOrEmpty(email.Subject)) {
                msg = "Subject is required";
            }

            if (String.IsNullOrEmpty(email.ToEmail)) {
                msg = "ToEmail is required";
            }

            if (String.IsNullOrEmpty(email.PlaintextBody) && String.IsNullOrEmpty(email.HtmlBody)) {
                msg = "Either HtmlBody or PlaintextBody is required";
            }

            if (CustomHeaders.ContainsKey("message-id")) {
                msg = "The message-id header is reserved for internal use";
            }

            if (msg.Length > 0) {
                Logger.error(msg);
                throw new MessageBusValidationFailedException(msg);
            }

            if (!String.IsNullOrEmpty(TemplateKey)) {
                Logger.warning("'TemplateKey' is ignored unless sending template email");
            }
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
