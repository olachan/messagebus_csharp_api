using MessageBus.API;
using MessageBus.API.V3;
using MessageBus.API.V3.Debug;
using MessageBus.Impl;
using MessageBus.SPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MessageBusTest.Impl {

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
            Client = new AutoBatchingClient("TEST_KEY", MockHttpClient, MockLogger);
        }


        [TestMethod]
        public void CanRetrieveApiKey() {
            Assert.AreEqual("TEST_KEY", Client.ApiKey);
        }

        [TestMethod]
        public void EmailBufferSizeDefaultsTo20AndCanBeChanged() {
            Assert.AreEqual(20, Client.EmailBufferSize);
            Client.EmailBufferSize = 100;
            Assert.AreEqual(100, Client.EmailBufferSize);
        }

        [TestMethod]
        public void CallingSendWithABufferSizeOfOneResultsInSend() {

            MockHttpClient.Expect(x => x.SendEmails(Arg<BatchEmailSendRequest>.Is.Anything)).Return(new BatchEmailResponse {
                failureCount = 0,
                successCount = 1,
                statusMessage = "",
                results = new[] {
                    new BatchEmailResult {
                     messageId = "1234",
                     status   = "OK",
                     statusMessage = ""
                    }
                }
            });
            Client.EmailBufferSize = 1;
            (Client as IMessageBusDebugging).SkipValidation = true;

            Client.Transmitted += Transmitted;

            var result = Client.Send(new MessageBusEmail {
                Subject = "Test",
                ToEmail = "bob@example.com",
                PlaintextBody = "Plain Text",
                FromEmail = "alice@example.com",
                FromName = "Alice Sample",
                Tags = new[] { "TAGA", "TAGB" },
            });

            var args = MockHttpClient.GetArgumentsForCallsMadeOn(x => x.SendEmails(Arg<BatchEmailSendRequest>.Is.Anything));
            var request = args[0][0] as BatchEmailSendRequest;

            Assert.AreEqual(1, request.messages.Count);

            Assert.AreEqual("Test", request.messages[0].subject);
            Assert.AreEqual("bob@example.com", request.messages[0].toEmail);
            Assert.AreEqual("Plain Text", request.messages[0].plaintextBody);

            Assert.AreEqual("alice@example.com", request.messages[0].fromEmail);
            Assert.AreEqual("Alice Sample", request.messages[0].fromName);
            Assert.AreEqual("TAGA", request.messages[0].tags[0]);
            Assert.AreEqual("TAGB", request.messages[0].tags[1]);

            Assert.IsTrue(result);

            Assert.AreEqual(0, TestEvent.FailureCount);
            Assert.AreEqual(1, TestEvent.SuccessCount);
            Assert.AreEqual("1234", TestEvent.Statuses[0].MessageId);
        }

        [TestMethod]
        public void CallingSendWithBufferSizeOfTwoDoesNotSend() {
            MockHttpClient.Expect(x => x.SendEmails(Arg<BatchEmailSendRequest>.Is.Anything)).Repeat.Never();

            Client.EmailBufferSize = 2;
            (Client as IMessageBusDebugging).SkipValidation = true;

            Client.Send(new MessageBusEmail());
        }

        [TestMethod]
        public void CallingSendWithBufferSizeOfTwoDoesNotSendUntilFlush() {
            MockHttpClient.Expect(x => x.SendEmails(Arg<BatchEmailSendRequest>.Is.Anything)).Repeat.Once();

            Client.EmailBufferSize = 2;
            (Client as IMessageBusDebugging).SkipValidation = true;

            Client.Send(new MessageBusEmail());
            Assert.IsTrue(Client.Flush());
        }

        [TestMethod]
        public void CallingSendWithBufferSizeOfTwoInAUsingBlockFlushesAtTheEnd() {
            MockHttpClient.Expect(x => x.SendEmails(Arg<BatchEmailSendRequest>.Is.Anything)).Repeat.Once();

            Client.EmailBufferSize = 2;
            (Client as IMessageBusDebugging).SkipValidation = true;

            using (Client) {
                Client.Send(new MessageBusEmail());
            }
        }

        [TestMethod]
        public void CallingFlushWithNoQueueMessagesReturnsFalse() {
            Assert.IsFalse(Client.Flush());
        }

        [TestMethod]
        public void CanCastClientToAccessDebuggingOptions() {
            (Client as IMessageBusDebugging).Domain = "test.example.com";
        }

        [TestMethod]
        public void ValidatesEachCallToSend() {
            try {
                Client.Send(new MessageBusEmail());
            } catch (MessageBusValidationFailedException e) {
                return;
            }
            Assert.Fail("Expected Exception to be thrown");
        }

        [TestMethod]
        public void ValidationPassesIfAllRequiredFieldsAreSupplied() {
            Client.Send(new MessageBusEmail {
                FromEmail = "alice@example.com",
                ToEmail = "bob@example.com",
                Subject = "Test",
                PlaintextBody = "Plain Text Email Body"
            });
        }

        [TestMethod]
        public void ChecksForThePresenceOfMergeFieldsWhenATemplateKeyIsSpecifiedAndWorksIfPresent() {
            
            var email = new MessageBusTemplateEmail {
                TemplateKey = "TEST",
                ToEmail = "alice@example.com"
            };
            email.MergeFields.Add("%EMAIL%", "bob@example.com");
            Client.Send(email);
        }

        [TestMethod]
        public void ThrowsAnErrorIfEmailMergeFieldIfMissing() {
            try {
                
                var email = new MessageBusEmail {
                    ToEmail = "bob@example.com",
                    Subject = "Test",
                    FromEmail = "alice@example.com"
                };
                Client.Send(email);
            } catch (MessageBusValidationFailedException e) {
                return;
            }
            Assert.Fail("Expected Exception to be thrown");
        }

        [TestMethod]
        public void ThrowsAnErrorIfTheSuppliedMergeFieldsDoNotStartAndEndWithPercentSymbols() {
            try {
                
                var email = new MessageBusTemplateEmail {
                    TemplateKey = "TEST",
                    ToEmail = "alice@example.com"
                };
                email.MergeFields.Add("EMAIL", "bob@example.com");
                Client.Send(email);
            } catch (MessageBusValidationFailedException e) {
                return;
            }
            Assert.Fail("Expected Exception to be thrown");
        }

        [TestMethod]
        public void ThrowsAnErrorIfACustomMessageIdHeaderIsSupplied() {
            try {
                var email = new MessageBusEmail {
                    FromEmail = "alice@example.com",
                    ToEmail = "bob@example.com",
                    Subject = "Test",
                    PlaintextBody = "Test"
                };
                email.CustomHeaders.Add("message-id", "some message id");
                Client.Send(email);
            } catch (MessageBusValidationFailedException e) {
                return;
            }
            Assert.Fail("Expected Exception to be thrown");
        }

        void Transmitted(IMessageBusTransmissionEvent transmissionEvent) {
            TestEvent = transmissionEvent;
        }
    }
}
