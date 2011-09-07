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

        private const string REQUEST_URL_FORMAT = "{0}/{1}/{2}";
        private const string SEND_EMAILS = "emails/send";

        private const string SEND_EMAILS_POST_FORMAT = "json={0}";

        public SimpleHttpClient() {
            Domain = "https://api.messagebus.com";
            Path = "api/v2";
            Serializer = new JavaScriptSerializer();
            Logger = new NullLogger();
        }
        public SimpleHttpClient(ILogger logger)
            : this() {
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

        public BatchEmailResponse SendEmails(BatchEmailRequest batchEmailRequest) {
            var uriString = String.Format(REQUEST_URL_FORMAT, Domain, Path, SEND_EMAILS);

            var request = CreateRequest(uriString);

            string postData = String.Format(SEND_EMAILS_POST_FORMAT, HttpUtility.UrlEncode(Serializer.Serialize(batchEmailRequest)));
            byte[] postDataArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataArray.Length;

            request.Method = "POST";
            using (var requestStream = request.GetRequestStream()) {
                requestStream.Write(postDataArray, 0, postDataArray.Length);
            }

            try {
                using (var response = WrapResponse(request.GetResponse())) {
                    using (var responseStream = response.GetResponseStream()) {
                        using (var reader = new StreamReader(responseStream, Encoding.UTF8)) {
                            string responseString = reader.ReadToEnd();
                            var result = Serializer.Deserialize<BatchEmailResponse>(responseString);
                            return result;
                        }
                    }
                }
            } catch (WebException e) {
                if (e.Response != null) {
                    BatchEmailResponse result;
                    using (var responseStream = e.Response.GetResponseStream()) {
                        using (var reader = new StreamReader(responseStream, Encoding.UTF8)) {
                            string responseString = reader.ReadToEnd();
                            result = Serializer.Deserialize<BatchEmailResponse>(responseString);
                        }
                    }
                    Logger.error(String.Format("Request Failed with Status: {0}. StatusMessage={1}. Message={2}", e.Status, result.statusMessage, e.Message));
                } else {
                    Logger.error(String.Format("Request Failed with Status: {0}. StatusMessage=<Unknown>. Message={1}", e.Status, e.Message));
                }
                throw;
            }
        }

        protected internal virtual WebRequestWrapper CreateRequest(String uriString) {
            var httpWebRequest = WebRequest.Create(new Uri(uriString)) as HttpWebRequest;
            if (httpWebRequest != null) {
                httpWebRequest.KeepAlive = true;
                httpWebRequest.AllowAutoRedirect = true;
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