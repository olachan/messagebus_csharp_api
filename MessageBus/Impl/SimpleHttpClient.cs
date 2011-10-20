using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using MessageBus.API;
using MessageBus.SPI;

namespace MessageBus.Impl {
    /// <summary>
    /// A Client that uses regular System.Net WebRequest objects to handle transmission.
    /// </summary>
    public class SimpleHttpClient : IMessageBusHttpClient {

        private readonly ILogger Logger;
        private readonly String ApiKey;

        private const string USER_AGENT = "MessageBusAPI:3.0.0-CSHARP:3.5";
        private const string REQUEST_URL_FORMAT = "{0}/{1}/{2}";
        private const string SEND_EMAILS = "emails/send";
        private const string SEND_TEMPLATE = "templates/send";
        private const string STATS = "stats";
        private const string DELIVERY_ERRORS = "delivery_errors";
        private const string UNSUBSCRIBES = "unsubscribes";
        private const string MAILING_LISTS = "mailing_lists";
        private const string MAILING_LIST_ENTRIES_FORMAT = "mailing_lists/{0}/entries";
        private const string MAILING_LIST_ENTRY_FORMAT = "mailing_lists/{0}/entry/{1}";

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

            var request = CreateRequest(uriString);

            string postData = Serializer.Serialize(batchEmailSendRequest);
            byte[] postDataArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/json";
            request.ContentLength = postDataArray.Length;

            request.Method = "POST";
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

            var request = CreateRequest(uriString);

            string postData = Serializer.Serialize(batchTemplateSendRequest);
            byte[] postDataArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/json";
            request.ContentLength = postDataArray.Length;

            request.Method = "POST";
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
                    uriString += "startDate=" + startDate.Value.ToString("yyyy-MM-dd") + "&";
                }
                if (endDate.HasValue) {
                    uriString += "endDate=" + endDate.Value.ToString("yyyy-MM-dd") + "&";
                }
                if (tag != null) {
                    uriString += "tag=" + tag + "&";
                }
                uriString = uriString.TrimEnd('&');
            }

            var request = CreateRequest(uriString);

            request.Method = "GET";

            try {
                return HandleResponse<StatsResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        public DeliveryErrorsResponse RetrieveDeliveryErrors(DateTime? startDate, DateTime? endDate) {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, DELIVERY_ERRORS);

            if (startDate.HasValue || endDate.HasValue) {
                uriString += "?";
                if (startDate.HasValue) {
                    uriString += "startDate=" + startDate.Value.ToString("yyyy-MM-dd") + "&";
                }
                if (endDate.HasValue) {
                    uriString += "endDate=" + endDate.Value.ToString("yyyy-MM-dd") + "&";
                }
                uriString = uriString.TrimEnd('&');
            }

            var request = CreateRequest(uriString);

            request.Method = "GET";

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
                    uriString += "startDate=" + startDate.Value.ToString("yyyy-MM-dd") + "&";
                }
                if (endDate.HasValue) {
                    uriString += "endDate=" + endDate.Value.ToString("yyyy-MM-dd") + "&";
                }
                uriString = uriString.TrimEnd('&');
            }

            var request = CreateRequest(uriString);

            request.Method = "GET";

            try {
                return HandleResponse<UnsubscribesResponse>(request);
            } catch (WebException e) {
                throw HandleException(e);
            }
        }

        public MailingListCreateResponse CreateMailingList(MailingListCreateRequest mailingListCreateRequest) {
            throw new NotImplementedException();
        }

        public MailingListsResponse ListMailingLists() {
            throw new NotImplementedException();
        }

        public MailingListEntryCreateRequest CreateMailingListEntry(string mailingListKey, MailingListEntryCreateRequest mailingListEntryCreateRequest) {
            throw new NotImplementedException();
        }

        public MailingListEntryDeleteResponse DeleteMailingListEntry(string mailingListKey, string emailAddress) {
            throw new NotImplementedException();
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

        private WebException HandleException(WebException e) {
            if (e.Response != null) {
                string message;
                using (var responseStream = e.Response.GetResponseStream()) {
                    using (var reader = new StreamReader(responseStream, Encoding.UTF8)) {
                        string responseString = reader.ReadToEnd();
                        try {
                            var result = Serializer.Deserialize<BatchEmailResponse>(responseString);
                            message = result.statusMessage;
                        } catch (ArgumentException x) {
                            message = responseString;
                        }
                    }
                }
                Logger.error(String.Format("Request Failed with Status: {0}. StatusMessage={1}. Message={2}", e.Status, message, e.Message));
            } else {
                Logger.error(String.Format("Request Failed with Status: {0}. StatusMessage=<Unknown>. Message={1}", e.Status, e.Message));
            }
            return e;
        }

        protected internal virtual WebRequestWrapper CreateRequest(String uriString) {
            var httpWebRequest = WebRequest.Create(new Uri(uriString)) as HttpWebRequest;
            if (httpWebRequest != null) {
                httpWebRequest.KeepAlive = true;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.Headers.Add("X-Messagebus-Key", ApiKey);
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