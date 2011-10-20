using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MessageBus.API;
using MessageBus.API.V3;
using MessageBus.API.V3.Debug;
using MessageBus.SPI;

namespace MessageBus.Impl {

    /// <summary>
    /// An implementation of IMessageBusEmailClient that automatically buffers and transmits email to the server in batches. 
    /// </summary>
    public class AutoBatchingEmailClient : IMessageBusEmailClient, IMessageBusDebugging {

        public event MessageTransmissionHandler Transmitted;

        public string ApiKey { get; private set; }

        private readonly ILogger Logger;
        private readonly IMessageBusHttpClient HttpClient;

        private BatchEmailSendRequest CurrentEmailSendRequest;
        private BatchTemplateSendRequest CurrentTemplateSendRequest;

        public AutoBatchingEmailClient(string apiKey)
            : this(apiKey, new SimpleHttpClient(apiKey), new NullLogger()) {
        }

        public AutoBatchingEmailClient(string apiKey, IMessageBusHttpClient httpClient)
            : this(apiKey, httpClient, new NullLogger()) {
        }

        public AutoBatchingEmailClient(string apiKey, ILogger logger)
            : this(apiKey, new SimpleHttpClient(apiKey, logger), logger) {
        }

        public AutoBatchingEmailClient(string apiKey, IMessageBusHttpClient httpClient, ILogger logger) {
            ApiKey = apiKey;
            HttpClient = httpClient;
            Logger = logger;
            EmailBufferSize = 20;
            Logger.info(String.Format("AutoBatchingEmailClient created with http EmailClient class {0}", httpClient.GetType().Name));
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
                if (CurrentEmailSendRequest != null) {
                    var response = HttpClient.SendEmails(CurrentEmailSendRequest);
                    OnTranmission(response);
                    result = CurrentEmailSendRequest.messages.Count;
                    CurrentEmailSendRequest = new BatchEmailSendRequest();
                }
                if (CurrentTemplateSendRequest != null) {
                    var response = HttpClient.SendEmails(CurrentTemplateSendRequest);
                    OnTranmission(response);
                    result = CurrentTemplateSendRequest.messages.Count;
                    CurrentTemplateSendRequest = new BatchTemplateSendRequest();
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
            return Send(new BatchTemplateMessage(email));
        }

        private bool Send(BatchEmailMessage email) {

            lock (this) {
                var result = 0;
                if (CurrentEmailSendRequest == null) {
                    CurrentEmailSendRequest = new BatchEmailSendRequest();
                }

                CurrentEmailSendRequest.messages.Add(email);

                if (CurrentEmailSendRequest.messages.Count >= EmailBufferSize) {
                    var response = HttpClient.SendEmails(CurrentEmailSendRequest);
                    OnTranmission(response);
                    result = CurrentEmailSendRequest.messages.Count;
                    CurrentEmailSendRequest = new BatchEmailSendRequest();
                }
                Logger.info(String.Format("Send Complete: {0} messages buffered and {1} messages tranmitted.", 1, result));
                return result > 0;
            }
        }

        private bool Send(BatchTemplateMessage email) {

            lock (this) {
                var result = 0;
                if (CurrentTemplateSendRequest == null) {
                    CurrentTemplateSendRequest = new BatchTemplateSendRequest();
                }

                CurrentTemplateSendRequest.messages.Add(email);

                if (CurrentTemplateSendRequest.messages.Count >= EmailBufferSize) {
                    var response = HttpClient.SendEmails(CurrentTemplateSendRequest);
                    OnTranmission(response);
                    result = CurrentTemplateSendRequest.messages.Count;
                    CurrentTemplateSendRequest = new BatchTemplateSendRequest();
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

        public void Dispose() {
            Flush();
        }

        private void Validate(MessageBusTemplateEmail email) {
            if (SkipValidation) return;
            string msg = "";

            if (String.IsNullOrEmpty(ApiKey)) {
                msg = "ApiKey is required";
            }
            if (String.IsNullOrEmpty(email.TemplateKey)) {
                msg = "A TemplateKey must be supplied when sending templated email";
            }

            if (String.IsNullOrEmpty(email.ToEmail)) {
                msg = "A ToEmail must be supplied when sending templated email";
            }

            if (email.MergeFields.Count > 0) {
                if (email.MergeFields.Any(pair => !pair.Key.StartsWith("%") || !pair.Key.EndsWith("%"))) {
                    msg = "Merge Fields must be surrounded with %% e.g. %FIELD%";
                }
            }

            if (email.CustomHeaders.ContainsKey("message-id")) {
                msg = "The message-id header is reserved for internal use";
            }

            if (msg.Length > 0) {
                Logger.error(msg);
                throw new MessageBusValidationFailedException(msg);
            }

        }

        private void Validate(MessageBusEmail email) {
            if (SkipValidation) return;
            string msg = "";

            if (String.IsNullOrEmpty(ApiKey)) {
                msg = "ApiKey is required";
            }

            if (String.IsNullOrEmpty(email.FromEmail)) {
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

            if (email.CustomHeaders.ContainsKey("message-id")) {
                msg = "The message-id header is reserved for internal use";
            }

            if (msg.Length > 0) {
                Logger.error(msg);
                throw new MessageBusValidationFailedException(msg);
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
            Succeeded = (response.status == "200");
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
