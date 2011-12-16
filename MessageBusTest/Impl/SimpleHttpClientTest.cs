// Copyright (c) 2011. Message Bus
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using MessageBus.API;
using MessageBus.API.V3;
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
            Client = MockRepository.GeneratePartialMock<SimpleHttpClient>("AnAPIKey", Logger);
            Request = MockRepository.GenerateMock<WebRequestWrapper>();
            Response = MockRepository.GenerateMock<WebResponseWrapper>();

            ReqStream = new MemoryStream();
            RespStream = new MemoryStream();
        }

        private void SetupDefaultExpectations() {
            Request.Expect(x => x.GetRequestStream()).Return(ReqStream);
            Request.Expect(x => x.GetResponse()).Return(null);

            Response.Expect(x => x.GetResponseStream()).Return(RespStream);
            Response.Expect(x => x.Dispose());

            Client.Expect(x => x.CreateRequest(Arg<string>.Is.Anything, Arg<SimpleHttpClient.HttpMethod>.Is.Anything)).Return(Request);
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

            var testRequest = new BatchEmailSendRequest {
            };

            testRequest.messages.Add(new BatchEmailMessage {
                fromEmail = "test@example.com",
                fromName = "Test Sender",
                tags = new[] { "test", "test2" },
                toEmail = "bob@example.com",
                subject = "Test Subject",
                plaintextBody = "Plain Text",
                htmlBody = "<html><body>HTML</body></html>"
            });

            testRequest.messages[0].customHeaders.Add("Test", "Header");

            var testResponse = new BatchEmailResponse {
                statusMessage = "OK",
                successCount = 1,
                failureCount = 0,
                results = new[] {
                    new BatchEmailResult { messageStatus = 0, messageId = "1234ABCD1234ABCD", toEmail = "bob@example.com"}
                }
            };

            ResponseString = new JavaScriptSerializer().Serialize(testResponse);

            var response = Client.SendEmails(testRequest);
            Assert.AreEqual("https://api.messagebus.com/api/v3/emails/send", Client.GetArgumentsForCallsMadeOn(x => x.CreateRequest(Arg<String>.Is.Anything, Arg<SimpleHttpClient.HttpMethod>.Is.Anything))[0][0]);
            var expectedJson = @"{""messages"":[{""toEmail"":""bob@example.com"",""fromEmail"":""test@example.com"",""toName"":null,""fromName"":""Test Sender"",""subject"":""Test Subject"",""plaintextBody"":""Plain Text"",""htmlBody"":""\u003chtml\u003e\u003cbody\u003eHTML\u003c/body\u003e\u003c/html\u003e"",""customHeaders"":{""Test"":""Header""},""tags"":[""test"",""test2""]}]}";
            Assert.AreEqual(expectedJson, HttpUtility.UrlDecode(RequestString));
            Assert.AreEqual(testResponse.statusMessage, response.statusMessage);
            Assert.AreEqual(testResponse.successCount, response.successCount);
            Assert.AreEqual(testResponse.results[0].messageId, response.results[0].messageId);
        }

        [TestMethod]
        public void CanSetDomain() {
            SetupDefaultExpectations();

            Client.Domain = "https://test.somewhere.org";
            Client.SendEmails(new BatchEmailSendRequest());
            Assert.AreEqual("https://test.somewhere.org/api/v3/emails/send", Client.GetArgumentsForCallsMadeOn(x => x.CreateRequest(Arg<String>.Is.Anything, Arg<SimpleHttpClient.HttpMethod>.Is.Anything))[0][0]);
        }

        [TestMethod]
        public void CanSetPath() {
            SetupDefaultExpectations();

            Client.Path = "test/path";
            Client.SendEmails(new BatchEmailSendRequest());
            Assert.AreEqual("https://api.messagebus.com/test/path/emails/send", Client.GetArgumentsForCallsMadeOn(x => x.CreateRequest(Arg<String>.Is.Anything, Arg<SimpleHttpClient.HttpMethod>.Is.Anything))[0][0]);
        }

        [TestMethod]
        public void LogsWebErrors() {
            Request.Expect(x => x.GetRequestStream()).Return(ReqStream);
            Request.Expect(x => x.GetResponse()).Throw(new WebException("Some Message"));

            Client.Expect(x => x.CreateRequest(Arg<string>.Is.Anything, Arg<SimpleHttpClient.HttpMethod>.Is.Equal(SimpleHttpClient.HttpMethod.POST))).Return(Request);
            Logger.Expect(x => x.error("Request Failed with Status: UnknownError. StatusMessage=<Unknown>. Message=Some Message"));

            try {
                Client.SendEmails(new BatchEmailSendRequest());
            } catch (MessageBusException) {
                return;
            }
            Assert.Fail("Exception Expected");
        }
    }
}
