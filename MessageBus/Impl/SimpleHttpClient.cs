// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using MessageBus.API;
using MessageBus.API.V3;
using MessageBus.SPI;

namespace MessageBus.Impl {
    /// <summary>
    /// A Client that uses regular System.Net WebRequest objects to handle transmission.
    /// </summary>
    public class SimpleHttpClient : IMessageBusHttpClient {

        private readonly ILogger Logger;
        private readonly String ApiKey;

        private const string USER_AGENT = "MessageBusAPI:3.0.2-CSHARP:3.5";
        private const string REQUEST_URL_FORMAT = "{0}/{1}/{2}";
        private const string SEND_EMAILS = "emails/send";
        private const string SEND_TEMPLATE = "templates/send";
        private const string STATS = "stats";
        private const string DELIVERY_ERRORS = "delivery_errors";
        private const string UNSUBSCRIBES = "unsubscribes";
        private const string FEEDBACKLOOPS = "feedbackloops";
        private const string MAILING_LISTS = "mailing_lists";
        private const string MAILING_LIST_ENTRIES_FORMAT = "mailing_list/{0}/entries";
        private const string MAILING_LIST_ENTRY_FORMAT = "mailing_list/{0}/entry/{1}";
        private const string MAILING_LIST_UPLOAD = "mailing_lists/upload";
        private const string CAMPAIGNS_SEND = "campaigns/send";
        private const string ISO_8601_DATE_FORMAT = "yyyy-MM-ddTHH:mm:ssZ";
        private const string MIME_BOUNDARY = "AaBt03x";
        private const int UPLOAD_BUFFER_SIZE = 4096;

        public enum HttpMethod {
            GET, POST, DELETE, PUT
        }

        public SimpleHttpClient(String apiKey) {
            Domain = "https://api.messagebus.com";
            Path = "api/v3";
            Serializer = new JavaScriptSerializer();
            Logger = new NullLogger();
            ApiKey = apiKey;
        }
        public SimpleHttpClient(String apiKey, ILogger logger)
            : this(apiKey) {
            Logger = logger;
        }

        public JavaScriptSerializer Serializer { get; set; }
        public string Domain { private get; set; }
        public string Path { private get; set; }

        public bool SslVerifyPeer {
            set {
                if (value == false) {
                    ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                }
            }
        }

        public IWebProxy Proxy { private get; set; }
        public ICredentials Credentials { private get; set; }

        public BatchEmailResponse SendEmails(BatchEmailSendRequest batchEmailSendRequest) {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, SEND_EMAILS);

            var request = CreateRequest(uriString, HttpMethod.POST);

            string postData = Serializer.Serialize(batchEmailSendRequest);
            byte[] postDataArray = Encoding.UTF8.GetBytes(postData);


            request.ContentLength = postDataArray.Length;

            using (var requestStream = request.GetRequestStream()) {
                requestStream.Write(postDataArray, 0, postDataArray.Length);
            }

            try {
                return HandleResponse<BatchEmailResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        public BatchEmailResponse SendEmails(BatchTemplateSendRequest batchTemplateSendRequest) {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, SEND_TEMPLATE);

            var request = CreateRequest(uriString, HttpMethod.POST);

            string postData = Serializer.Serialize(batchTemplateSendRequest);
            byte[] postDataArray = Encoding.UTF8.GetBytes(postData);

            request.ContentLength = postDataArray.Length;

            using (var requestStream = request.GetRequestStream()) {
                requestStream.Write(postDataArray, 0, postDataArray.Length);
            }

            try {
                return HandleResponse<BatchEmailResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        public StatsResponse RetrieveStats(DateTime? startDate, DateTime? endDate, string tag) {

            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, STATS);

            if (startDate.HasValue || endDate.HasValue || tag != null) {
                uriString += "?";
                if (startDate.HasValue) {
                    uriString += "startDate=" + startDate.Value.ToString(ISO_8601_DATE_FORMAT) + "&";
                }
                if (endDate.HasValue) {
                    uriString += "endDate=" + endDate.Value.ToString(ISO_8601_DATE_FORMAT) + "&";
                }
                if (tag != null) {
                    uriString += "tag=" + tag + "&";
                }
                uriString = uriString.TrimEnd('&');
            }

            var request = CreateRequest(uriString, HttpMethod.GET);

            try {
                return HandleResponse<StatsResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        public DeliveryErrorsResponse RetrieveDeliveryErrors(DateTime? startDate, DateTime? endDate, string tag) {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, DELIVERY_ERRORS);

            if (startDate.HasValue || endDate.HasValue || tag != null) {
                uriString += "?";
                if (startDate.HasValue) {
                    uriString += "startDate=" + startDate.Value.ToString(ISO_8601_DATE_FORMAT) + "&";
                }
                if (endDate.HasValue) {
                    uriString += "endDate=" + endDate.Value.ToString(ISO_8601_DATE_FORMAT) + "&";
                }
                if (tag != null) {
                    uriString += "tag=" + tag + "&";
                }
                uriString = uriString.TrimEnd('&');
            }

            var request = CreateRequest(uriString, HttpMethod.GET);

            try {
                return HandleResponse<DeliveryErrorsResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        public UnsubscribesResponse RetrieveUnsubscribes(DateTime? startDate, DateTime? endDate) {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, UNSUBSCRIBES);

            if (startDate.HasValue || endDate.HasValue) {
                uriString += "?";
                if (startDate.HasValue) {
                    uriString += "startDate=" + startDate.Value.ToString(ISO_8601_DATE_FORMAT) + "&";
                }
                if (endDate.HasValue) {
                    uriString += "endDate=" + endDate.Value.ToString(ISO_8601_DATE_FORMAT) + "&";
                }
                uriString = uriString.TrimEnd('&');
            }

            var request = CreateRequest(uriString, HttpMethod.GET);

            try {
                return HandleResponse<UnsubscribesResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        public FeedbackloopsResponse RetrieveFeedbackloops(DateTime? startDate, DateTime? endDate) {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, FEEDBACKLOOPS);

            if (startDate.HasValue || endDate.HasValue) {
                uriString += "?";
                if (startDate.HasValue) {
                    uriString += "startDate=" + startDate.Value.ToString(ISO_8601_DATE_FORMAT) + "&";
                }
                if (endDate.HasValue) {
                    uriString += "endDate=" + endDate.Value.ToString(ISO_8601_DATE_FORMAT) + "&";
                }
                uriString = uriString.TrimEnd('&');
            }

            var request = CreateRequest(uriString, HttpMethod.GET);

            try {
                return HandleResponse<FeedbackloopsResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        public MailingListsResponse ListMailingLists() {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, MAILING_LISTS);

            var request = CreateRequest(uriString, HttpMethod.GET);

            try {
                return HandleResponse<MailingListsResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        public MailingListUploadResponse UploadMailingList(MailingListUploadRequest mailingListUploadRequest, MailingListUploadProgressHandler onUploadProgress) {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, MAILING_LIST_UPLOAD);
            var request = CreateRequest(uriString, HttpMethod.POST);
            request.SendChunked = true;
            request.Timeout = Timeout.Infinite;
            request.AllowWriteStreamBuffering = false;

            var boundary = Encoding.UTF8.GetBytes(String.Format("--{0}\r\n", MIME_BOUNDARY));
            var eof = Encoding.UTF8.GetBytes(String.Format("--{0}--\r\n", MIME_BOUNDARY));
            var newLine = Encoding.UTF8.GetBytes("\r\n");

            string part1Json = Serializer.Serialize(mailingListUploadRequest);
            byte[] part1 = Encoding.UTF8.GetBytes(part1Json);

            request.ContentType = String.Format("multipart/form-data; boundary={0}", MIME_BOUNDARY);
            var totalBytes = mailingListUploadRequest.fileInfo.Length;

            var part1Headers = Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"jsonData\";\r\nContent-Type: application/json; charset=utf-8;\r\n\r\n");
            var part2Headers = Encoding.UTF8.GetBytes(String.Format("Content-Disposition: form-data; name=\"fileData\"; filename=\"{0}\";\r\nContent-Type: application/x-gzip\r\n\r\n", mailingListUploadRequest.fileInfo.Name));

            using (var requestStream = request.GetRequestStream()) {
                requestStream.WriteTimeout = Timeout.Infinite;
                requestStream.Write(boundary, 0, boundary.Length);
                requestStream.Write(part1Headers, 0, part1Headers.Length);
                requestStream.Write(part1, 0, part1.Length);
                requestStream.Write(newLine, 0, newLine.Length);
                requestStream.Write(boundary, 0, boundary.Length);
                requestStream.Write(part2Headers, 0, part2Headers.Length);
                var gzipStream = new GZipStream(requestStream, CompressionMode.Compress, true);
                var uploadedBytes = 0L;
                using (var fs = mailingListUploadRequest.fileInfo.OpenRead()) {
                    var buff = new byte[UPLOAD_BUFFER_SIZE];
                    var len = 0;
                    while ((len = fs.Read(buff, 0, buff.Length)) > 0) {
                        gzipStream.Write(buff, 0, len);
                        uploadedBytes += len;
                        if (onUploadProgress != null) {
                            onUploadProgress(uploadedBytes, totalBytes);
                        }
                    }
                }
                gzipStream.Close();
                requestStream.Write(newLine, 0, newLine.Length);
                requestStream.Write(eof, 0, eof.Length);
                requestStream.Flush();
            }

            try {
                return HandleResponse<MailingListUploadResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }

        }

        public CampaignSendResponse SendCampaign(CampaignSendRequest campaignSendRequest) {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, CAMPAIGNS_SEND);

            var request = CreateRequest(uriString, HttpMethod.POST);

            string postData = Serializer.Serialize(campaignSendRequest);
            byte[] postDataArray = Encoding.UTF8.GetBytes(postData);

            request.ContentLength = postDataArray.Length;

            using (var requestStream = request.GetRequestStream()) {
                requestStream.Write(postDataArray, 0, postDataArray.Length);
            }

            try {
                return HandleResponse<CampaignSendResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        public MailingListEntryCreateResponse CreateMailingListEntry(string mailingListKey, MailingListEntryCreateRequest mailingListEntryCreateRequest) {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, String.Format(MAILING_LIST_ENTRIES_FORMAT, mailingListKey));

            var request = CreateRequest(uriString, HttpMethod.POST);

            string postData = Serializer.Serialize(mailingListEntryCreateRequest);
            byte[] postDataArray = Encoding.UTF8.GetBytes(postData);

            request.ContentLength = postDataArray.Length;

            using (var requestStream = request.GetRequestStream()) {
                requestStream.Write(postDataArray, 0, postDataArray.Length);
            }

            try {
                return HandleResponse<MailingListEntryCreateResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        public MailingListEntryDeleteResponse DeleteMailingListEntry(string mailingListKey, string emailAddress) {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, String.Format(MAILING_LIST_ENTRY_FORMAT, mailingListKey, emailAddress));

            var request = CreateRequest(uriString, HttpMethod.DELETE);

            try {
                return HandleResponse<MailingListEntryDeleteResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        private T HandleResponse<T>(WebRequestWrapper request) {
            using (var response = WrapResponse(request.GetResponse())) {
                using (var responseStream = response.GetResponseStream()) {
                    using (var reader = new StreamReader(responseStream, Encoding.UTF8)) {
                        string responseString = reader.ReadToEnd();
                        var result = Serializer.Deserialize<T>(responseString);
                        return result;
                    }
                }
            }
        }

        private MessageBusException HandleException(WebException e) {
            if (e.Response != null) {
                var httpWebResponse = e.Response as HttpWebResponse;
                if (httpWebResponse != null) {
                    string message;
                    using (var responseStream = httpWebResponse.GetResponseStream()) {
                        if (responseStream.CanRead) {
                            using (var reader = new StreamReader(responseStream, Encoding.UTF8)) {
                                string responseString = reader.ReadToEnd();
                                try {
                                    var result = Serializer.Deserialize<ErrorResponse>(responseString);
                                    message = result.statusMessage;
                                } catch (ArgumentException) {
                                    message = responseString;
                                }
                            }
                        } else {
                            message = httpWebResponse.StatusDescription;
                        }
                    }
                    Logger.error(String.Format("Request Failed with Status: {0}. StatusMessage={1}. Message={2}",
                                               httpWebResponse.StatusCode, httpWebResponse.StatusDescription, message));
                    return new MessageBusException((int)httpWebResponse.StatusCode, message);
                }
                Logger.error(String.Format("Request Failed with Status: {0}. StatusMessage=<Unknown>. Message={1}", e.Status, e.Message));
                return new MessageBusException(e);
            }
            Logger.error(String.Format("Request Failed with Status: {0}. StatusMessage=<Unknown>. Message={1}", e.Status, e.Message));
            return new MessageBusException(e);
        }

        protected internal virtual WebRequestWrapper CreateRequest(String uriString, HttpMethod method) {
            var httpWebRequest = WebRequest.Create(new Uri(uriString)) as HttpWebRequest;
            if (httpWebRequest != null) {
                httpWebRequest.Method = method.ToString();
                httpWebRequest.KeepAlive = false;
                httpWebRequest.AllowAutoRedirect = true;
                if (method == HttpMethod.POST) {
                    httpWebRequest.ContentType = "application/json; charset=utf-8";
                }
                httpWebRequest.Headers.Add("X-MessageBus-Key", ApiKey);
                httpWebRequest.UserAgent = USER_AGENT;
                if (Proxy != null) {
                    httpWebRequest.Proxy = Proxy;
                }
                if (Credentials != null) {
                    httpWebRequest.Credentials = Credentials;
                }
                return new WebRequestWrapper(httpWebRequest);
            }
            throw new ApplicationException("Could not create web request");
        }

        private void SetBasicAuthHeader(WebRequest req, String userName, String userPassword) {
            string authInfo = userName + ":" + userPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            req.Headers["Authorization"] = "Basic " + authInfo;
        }

        protected internal virtual WebResponseWrapper WrapResponse(WebResponse response) {
            return new WebResponseWrapper(response as HttpWebResponse);
        }
    }

    /// <summary>
    /// Virtualized test wrapper to allow for mocking
    /// </summary>
    public class WebRequestWrapper {
        private readonly HttpWebRequest InnerRequest;

        protected internal WebRequestWrapper(HttpWebRequest req) {
            InnerRequest = req;
        }

        protected internal WebRequestWrapper() {
            InnerRequest = null;
        }

        public virtual string ContentType {
            get { return InnerRequest.ContentType; }
            set { InnerRequest.ContentType = value; }
        }

        public virtual long ContentLength {
            get { return InnerRequest.ContentLength; }
            set { InnerRequest.ContentLength = value; }
        }

        public virtual string Method {
            get { return InnerRequest.Method; }
            set { InnerRequest.Method = value; }
        }

        public virtual Stream GetRequestStream() {
            return InnerRequest.GetRequestStream();
        }

        public virtual WebResponse GetResponse() {
            return InnerRequest.GetResponse();
        }

        public virtual bool SendChunked {
            get { return InnerRequest.SendChunked; }
            set { InnerRequest.SendChunked = value; }
        }

        public virtual int Timeout {
            get { return InnerRequest.Timeout; }
            set { InnerRequest.Timeout = value; }
        }

        public virtual bool AllowWriteStreamBuffering {
            get { return InnerRequest.AllowWriteStreamBuffering; }
            set { InnerRequest.AllowWriteStreamBuffering = value; }
        }
    }

    /// <summary>
    /// Virtualized Response Wrapper to allow for mocking
    /// </summary>
    public class WebResponseWrapper : IDisposable {
        private readonly HttpWebResponse InnerResponse;

        protected internal WebResponseWrapper(HttpWebResponse innerResponse) {
            InnerResponse = innerResponse;
        }

        protected internal WebResponseWrapper() {
            InnerResponse = null;
        }

        public virtual Stream GetResponseStream() {
            return InnerResponse.GetResponseStream();
        }

        public virtual HttpStatusCode StatusCode {
            get { return InnerResponse.StatusCode; }
        }

        public virtual string StatusDescription {
            get { return InnerResponse.StatusDescription; }
        }

        public virtual void Dispose() {
            ((IDisposable)InnerResponse).Dispose();
        }
    }
}