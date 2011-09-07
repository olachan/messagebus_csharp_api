using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using MessageBus.API;
using MessageBus.Impl;
using MessageBus.SPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Impl;

namespace MessageBusTest.Impl {
    [TestClass]
    public class SimpleHttpClientTest {

        public TestContext TestContext { get; set; }

        private SimpleHttpClient Client;
        private WebRequestWrapper Request;
        private WebResponseWrapper Response;

        private MemoryStream ReqStream;
        private MemoryStream RespStream;
        private ILogger Logger;

        public SimpleHttpClientTest() {
            RhinoMocks.Logger = new TextWriterExpectationLogger(Console.Out);
        }

        [TestInitialize()]
        public void TestInitialize() {
            Logger = MockRepository.GenerateMock<ILogger>();
            Client = MockRepository.GeneratePartialMock<SimpleHttpClient>(Logger);
            Request = MockRepository.GenerateMock<WebRequestWrapper>();
            Response = MockRepository.GenerateMock<WebResponseWrapper>();

            ReqStream = new MemoryStream();
            RespStream = new MemoryStream();
        }

        private void SetupDefaultExpectations() {
            Request.Expect(x => x.Method).SetPropertyWithArgument("POST");
            Request.Expect(x => x.ContentType).SetPropertyWithArgument("application/x-www-form-urlencoded");
            Request.Expect(x => x.GetRequestStream()).Return(ReqStream);
            Request.Expect(x => x.GetResponse()).Return(null);

            Response.Expect(x => x.GetResponseStream()).Return(RespStream);
            Response.Expect(x => x.Dispose());

            Client.Expect(x => x.CreateRequest(Arg<string>.Is.Anything)).Return(Request);
            Client.Expect(x => x.WrapResponse(Arg<WebResponse>.Is.Anything)).Return(Response);
        }

        [TestCleanup]
        public void TestCleanup() {
            Client.VerifyAllExpectations();
            Request.VerifyAllExpectations();
            Response.VerifyAllExpectations();
        }

        private string RequestString {
            get {
                return Encoding.UTF8.GetString(ReqStream.ToArray());
            }
        }

        private string ResponseString {
            set {
                var bytes = Encoding.UTF8.GetBytes(value);
                RespStream.Write(bytes, 0, bytes.Length);
                RespStream.Flush();
                RespStream.Position = 0;
            }
        }

        [TestMethod]
        public void MakesAValidApiRequest() {
            SetupDefaultExpectations();

            var testRequest = new BatchEmailRequest {
                apiKey = "ABCD-1234-ABCD-1234",
                apiVersion = "2.2",
                fromEmail = "test@example.com",
                fromName = "Test Sender",
                tags = new[] { "test", "test2" },
            };

            testRequest.messages.Add(new BatchEmailMessage {
                toEmail = "bob@example.com",
                plaintextBody = "Plain Text",
                htmlBody = "<html><body>HTML</body></html>",
            });

            var testResponse = new BatchEmailResponse {
                statusMessage = "OK",
                successCount = 1,
                failureCount = 0,
                results = new[] {
                    new BatchEmailResult { status = "OK", messageId = "1234ABCD1234ABCD" }
                }
            };

            ResponseString = new JavaScriptSerializer().Serialize(testResponse);

            var response = Client.SendEmails(testRequest);
            Assert.AreEqual("https://api.messagebus.com/api/v2/emails/send", Client.GetArgumentsForCallsMadeOn(x => x.CreateRequest(Arg<String>.Is.Anything))[0][0]);
            var expectedJson = @"json={""apiKey"":""ABCD-1234-ABCD-1234"",""apiVersion"":""2.2"",""templateKey"":null,""fromEmail"":""test@example.com"",""fromName"":""Test Sender"",""tags"":[""test"",""test2""],""customHeaders"":{},""messageCount"":1,""messages"":[{""toEmail"":""bob@example.com"",""subject"":null,""plaintextBody"":""Plain Text"",""htmlBody"":""\u003chtml\u003e\u003cbody\u003eHTML\u003c/body\u003e\u003c/html\u003e"",""mergeFields"":null}]}";
            Assert.AreEqual(expectedJson, HttpUtility.UrlDecode(RequestString));
            Assert.AreEqual(testResponse.statusMessage, response.statusMessage);
            Assert.AreEqual(testResponse.successCount, response.successCount);
            Assert.AreEqual(testResponse.results[0].messageId, response.results[0].messageId);
        }

        [TestMethod]
        public void CanSetDomain() {
            SetupDefaultExpectations();

            Client.Domain = "https://test.somewhere.org";
            Client.SendEmails(new BatchEmailRequest());
            Assert.AreEqual("https://test.somewhere.org/api/v2/emails/send", Client.GetArgumentsForCallsMadeOn(x => x.CreateRequest(Arg<String>.Is.Anything))[0][0]);
        }

        [TestMethod]
        public void CanSetPath() {
            SetupDefaultExpectations();

            Client.Path = "test/path";
            Client.SendEmails(new BatchEmailRequest());
            Assert.AreEqual("https://api.messagebus.com/test/path/emails/send", Client.GetArgumentsForCallsMadeOn(x => x.CreateRequest(Arg<String>.Is.Anything))[0][0]);
        }

        [TestMethod]
        public void LogsWebErrors() {
            Request.Expect(x => x.Method).SetPropertyWithArgument("POST");
            Request.Expect(x => x.ContentType).SetPropertyWithArgument("application/x-www-form-urlencoded");
            Request.Expect(x => x.GetRequestStream()).Return(ReqStream);
            Request.Expect(x => x.GetResponse()).Throw(new WebException("Some Message"));

            Client.Expect(x => x.CreateRequest(Arg<string>.Is.Anything)).Return(Request);
            Logger.Expect(x => x.error("Request Failed with Status: UnknownError. StatusMessage=<Unknown>. Message=Some Message"));

            try {
                Client.SendEmails(new BatchEmailRequest());
            } catch (WebException e) {
                return;
            }
            Assert.Fail("Exception Expected");
        }
    }
}
