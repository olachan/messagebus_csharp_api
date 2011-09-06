using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MessageBus.API;
using MessageBus.API.V2;
using MessageBus.API.V2.Debug;
using MessageBus.Impl;
using MessageBus.SPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MessageBusTest.Impl {
    /// <summary>
    /// Summary description for AutoBatchingClientTest
    /// </summary>
    [TestClass]
    public class AutoBatchingClientTest {
        private AutoBatchingClient Client;
        private IMessageBusHttpClient MockHttpClient;
        private ILogger MockLogger;
        private IMessageBusTransmissionEvent TestEvent;

        public TestContext TestContext { get; set; }

        [TestInitialize()]
        public void MyTestInitialize() {
            MockHttpClient = MockRepository.GenerateMock<IMessageBusHttpClient>();
            MockLogger = MockRepository.GenerateMock<ILogger>();
            Client = new AutoBatchingClient("TEST_KEY", "2.2", MockHttpClient, MockLogger);
        }


        [TestMethod]
        public void CanRetrieveApiKey() {
            Assert.AreEqual("TEST_KEY", Client.ApiKey);
        }

        [TestMethod]
        public void CanRetrieveApiVersion() {
            Assert.AreEqual("2.2", Client.ApiVersion);
        }

        [TestMethod]
        public void EmailBufferSizeDefaultsTo20AndCanBeChanged() {
            Assert.AreEqual(20, Client.EmailBufferSize);
            Client.EmailBufferSize = 100;
            Assert.AreEqual(100, Client.EmailBufferSize);
        }

        [TestMethod]
        public void CallingSendWithABufferSizeOfOneResultsInSend() {

            MockHttpClient.Expect(x => x.SendEmails(Arg<BatchEmailRequest>.Is.Anything)).Return(new BatchEmailResponse {
                failureCount = 0,
                successCount = 1,
                statusMessage = "",
                results = new[] {
                    new BatchEmailResult
                    {
                     messageId = "1234",
                     status   = "OK",
                     statusMessage = ""
                    }
                }
            });
            Client.EmailBufferSize = 1;
            Client.FromEmail = "alice@example.com";
            Client.FromName = "Alice Sample";
            Client.Tags = new[] { "TAGA", "TAGB" };

            Client.Transmitted += Transmitted;

            var result = Client.Send(new MessageBusEmail {
                Subject = "Test",
                ToEmail = "bob@example.com",
                ToName = "Bob Sample",
                PlaintextBody = "Plain Text"
            });

            var args = MockHttpClient.GetArgumentsForCallsMadeOn(x => x.SendEmails(Arg<BatchEmailRequest>.Is.Anything));
            var request = args[0][0] as BatchEmailRequest;

            Assert.AreEqual(1, request.messageCount);

            Assert.AreEqual("Test", request.messages[0].subject);
            Assert.AreEqual("bob@example.com", request.messages[0].toEmail);
            Assert.AreEqual("Bob Sample", request.messages[0].toName);
            Assert.AreEqual("Plain Text", request.messages[0].plaintextBody);

            Assert.AreEqual("TEST_KEY", request.apiKey);
            Assert.AreEqual("2.2", request.apiVersion);

            Assert.AreEqual("alice@example.com", request.fromEmail);
            Assert.AreEqual("Alice Sample", request.fromName);
            Assert.AreEqual("TAGA", request.tags[0]);
            Assert.AreEqual("TAGB", request.tags[1]);

            Assert.IsTrue(result);

            Assert.AreEqual(0, TestEvent.FailureCount);
            Assert.AreEqual(1, TestEvent.SuccessCount);
            Assert.AreEqual("1234", TestEvent.Statuses[0].MessageId);
        }

        [TestMethod]
        public void CallingSendWithBufferSizeOfTwoDoesNotSend() {
            MockHttpClient.Expect(x => x.SendEmails(Arg<BatchEmailRequest>.Is.Anything)).Repeat.Never();

            Client.EmailBufferSize = 2;

            Client.Send(new MessageBusEmail());
        }

        [TestMethod]
        public void CallingSendWithBufferSizeOfTwoDoesNotSendUntilFlush() {
            MockHttpClient.Expect(x => x.SendEmails(Arg<BatchEmailRequest>.Is.Anything)).Repeat.Once();

            Client.EmailBufferSize = 2;

            Client.Send(new MessageBusEmail());
            Assert.IsTrue(Client.Flush());
        }

        [TestMethod]
        public void CallingSendWithBufferSizeOfTwoInAUsingBlockFlushesAtTheEnd() {
            MockHttpClient.Expect(x => x.SendEmails(Arg<BatchEmailRequest>.Is.Anything)).Repeat.Once();

            Client.EmailBufferSize = 2;

            using (Client) {
                Client.Send(new MessageBusEmail());
            }
        }

        [TestMethod]
        public void CallingFlushWithNoQueueMessagesReturnsFalse()
        {
            Assert.IsFalse(Client.Flush());
        }

        [TestMethod]
        public void CanCastClientToAccessDebuggingOptions()
        {
            (Client as IMessageBusDebugging).Domain = "test.example.com";
        }

        void Transmitted(IMessageBusTransmissionEvent transmissionEvent) {
            TestEvent = transmissionEvent;
        }
    }
}
