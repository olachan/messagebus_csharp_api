using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using MessageBus.SPI;

namespace MessageBus.Impl {
    /// <summary>
    /// A Client that uses regular System.Net WebRequest objects to handle transmission.
    /// </summary>
    public class SimpleHttpClient : IMessageBusHttpClient {

        private const string REQUEST_URL_FORMAT = "{0}/{1}/{2}";
        private const string SEND_EMAILS = "emails/send";

        private const string SEND_EMAILS_POST_FORMAT = "json={0}";

        public SimpleHttpClient() {
            Domain = "https://api.messagebus.com";
            Path = "api/v2";
            Serializer = new JavaScriptSerializer();
        }

        public JavaScriptSerializer Serializer { get; set; }
        public string Domain { private get; set; }
        public string Path { private get; set; }

        public bool SslVerifyPeer {
            set {
                if (value) {
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

            using (var response = WrapResponse(request.GetResponse())) {
                using (var responseStream = response.GetResponseStream()) {
                    using (var reader = new StreamReader(responseStream, Encoding.UTF8)) {
                        string responseString = reader.ReadToEnd();
                        var result = Serializer.Deserialize<BatchEmailResponse>(responseString);
                        return result;
                    }
                }
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