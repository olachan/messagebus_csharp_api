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
        private AutoBatchingEmailClient EmailClient;
        private IMessageBusHttpClient MockHttpClient;
        private ILogger MockLogger;
        private IMessageBusTransmissionEvent TestEvent;

        public TestContext TestContext { get; set; }

        [TestInitialize()]
        public void MyTestInitialize() {
            MockHttpClient = MockRepository.GenerateMock<IMessageBusHttpClient>();
            MockLogger = MockRepository.GenerateMock<ILogger>();
            EmailClient = new AutoBatchingEmailClient("TEST_KEY", MockHttpClient, MockLogger);
        }


        [TestMethod]
        public void CanRetrieveApiKey() {
            Assert.AreEqual("TEST_KEY", EmailClient.ApiKey);
        }

        [TestMethod]
        public void EmailBufferSizeDefaultsTo20AndCanBeChanged() {
            Assert.AreEqual(20, EmailClient.EmailBufferSize);
            EmailClient.EmailBufferSize = 100;
            Assert.AreEqual(100, EmailClient.EmailBufferSize);
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
                     messageStatus = 0,
                     toEmail = "bob@example.com"
                    }
                }
            });
            EmailClient.EmailBufferSize = 1;
            (EmailClient as IMessageBusDebugging).SkipValidation = true;

            EmailClient.Transmitted += Transmitted;

            var result = EmailClient.Send(new MessageBusEmail {
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

            EmailClient.EmailBufferSize = 2;
            (EmailClient as IMessageBusDebugging).SkipValidation = true;

            EmailClient.Send(new MessageBusEmail());
        }

        [TestMethod]
        public void CallingSendWithBufferSizeOfTwoDoesNotSendUntilFlush() {
            MockHttpClient.Expect(x => x.SendEmails(Arg<BatchEmailSendRequest>.Is.Anything)).Repeat.Once();

            EmailClient.EmailBufferSize = 2;
            (EmailClient as IMessageBusDebugging).SkipValidation = true;

            EmailClient.Send(new MessageBusEmail());
            Assert.IsTrue(EmailClient.Flush());
        }

        [TestMethod]
        public void CallingSendWithBufferSizeOfTwoInAUsingBlockFlushesAtTheEnd() {
            MockHttpClient.Expect(x => x.SendEmails(Arg<BatchEmailSendRequest>.Is.Anything)).Repeat.Once();

            EmailClient.EmailBufferSize = 2;
            (EmailClient as IMessageBusDebugging).SkipValidation = true;

            using (EmailClient) {
                EmailClient.Send(new MessageBusEmail());
            }
        }

        [TestMethod]
        public void CallingFlushWithNoQueueMessagesReturnsFalse() {
            Assert.IsFalse(EmailClient.Flush());
        }

        [TestMethod]
        public void CanCastClientToAccessDebuggingOptions() {
            (EmailClient as IMessageBusDebugging).Domain = "test.example.com";
        }

        [TestMethod]
        public void ValidatesEachCallToSend() {
            try {
                EmailClient.Send(new MessageBusEmail());
            } catch (MessageBusValidationFailedException) {
                return;
            }
            Assert.Fail("Expected Exception to be thrown");
        }

        [TestMethod]
        public void ValidationPassesIfAllRequiredFieldsAreSupplied() {
            EmailClient.Send(new MessageBusEmail {
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
            EmailClient.Send(email);
        }

        [TestMethod]
        public void ThrowsAnErrorIfEmailMergeFieldIfMissing() {
            try {

                var email = new MessageBusEmail {
                    ToEmail = "bob@example.com",
                    Subject = "Test",
                    FromEmail = "alice@example.com"
                };
                EmailClient.Send(email);
            } catch (MessageBusValidationFailedException) {
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
                EmailClient.Send(email);
            } catch (MessageBusValidationFailedException) {
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
                EmailClient.Send(email);
            } catch (MessageBusValidationFailedException) {
                return;
            }
            Assert.Fail("Expected Exception to be thrown");
        }

        void Transmitted(IMessageBusTransmissionEvent transmissionEvent) {
            TestEvent = transmissionEvent;
        }
    }
}
