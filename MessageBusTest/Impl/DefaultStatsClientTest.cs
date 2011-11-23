using System.Collections.Generic;
using System.Net;
using MessageBus.API;
using MessageBus.Impl;
using MessageBus.SPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MessageBus.API.V3;
using Rhino.Mocks;

namespace MessageBusTest.Impl {

    [TestClass()]
    public class DefaultStatsClientTest {

        private DefaultStatsClient StatsClient;
        private IMessageBusHttpClient MockHttpClient;
        private ILogger MockLogger;

        public TestContext TestContext { get; set; }

        [TestInitialize()]
        public void MyTestInitialize() {
            MockHttpClient = MockRepository.GenerateMock<IMessageBusHttpClient>();
            MockLogger = MockRepository.GenerateMock<ILogger>();
            StatsClient = new DefaultStatsClient(MockHttpClient, MockLogger);
        }

        [TestMethod()]
        public void RetrieveDeliveryErrorsTest() {
            var result = new DeliveryErrorsResponseResult {
                DSNCode = "403",
                messageId = "TEST",
                time = DateTime.Now,
                toEmail = "test@example.com"
            };
            MockHttpClient.Expect(
                x =>
                x.RetrieveDeliveryErrors(
                    Arg<DateTime>.Is.Equal(DateTime.Today.AddDays(-1)),
                    Arg<DateTime>.Is.Equal(DateTime.Today)))
                .Return(new DeliveryErrorsResponse {
                    statusCode = 200,
                    results = new List<DeliveryErrorsResponseResult> {
                        result
                    }
                });
            var startDate = DateTime.Today.AddDays(-1);
            var endDate = DateTime.Today;
            var actual = StatsClient.RetrieveDeliveryErrors(startDate, endDate);
            Assert.AreEqual("403", actual[0].DSNCode);
            Assert.AreEqual("TEST", actual[0].MessageId);
            Assert.AreEqual("test@example.com", actual[0].ToEmail);
        }

        [TestMethod()]
        public void RetrieveDeliveryErrorsTestThrowExceptionOnFailure() {
            MockHttpClient.Expect(
                x =>
                x.RetrieveDeliveryErrors(
                    Arg<DateTime>.Is.Anything,
                    Arg<DateTime>.Is.Anything))
                .Return(new DeliveryErrorsResponse {
                    statusCode = 503,
                    statusMessage = "Service Temporarily Unavailable"
                });
            try {
                StatsClient.RetrieveDeliveryErrors(null, null);
            } catch (MessageBusException e) {
                Assert.AreEqual(503, e.StatusCode);
                Assert.IsTrue(e.IsRetryable());
                return;
            }
            Assert.Fail("MessageBusException not raised");
        }

        [TestMethod()]
        public void RetrieveDeliveryErrorsTestThrowsWrappedExceptionOnWebException() {
            MockHttpClient.Expect(
                x =>
                x.RetrieveDeliveryErrors(
                    Arg<DateTime>.Is.Anything,
                    Arg<DateTime>.Is.Anything))
                .Throw(new MessageBusException(new WebException("Connection Rufused", WebExceptionStatus.ConnectFailure)));
            try {
                StatsClient.RetrieveDeliveryErrors(null, null);
            } catch (MessageBusException e) {
                Assert.AreEqual(-1, e.StatusCode);
                Assert.IsFalse(e.IsRetryable());
                return;
            }
            Assert.Fail("MessageBusException not raised");
        }

        [TestMethod()]
        public void RetrieveStatsTest() {
            var result = new StatsResponseResult() {
                clicks = 1,
                errors = 2,
                opens = 3,
                sent = 4,
                uniqueOpens = 5,
                date = DateTime.Now

            };
            MockHttpClient.Expect(
                x =>
                x.RetrieveStats(
                    Arg<DateTime>.Is.Equal(DateTime.Today.AddDays(-1)),
                    Arg<DateTime>.Is.Equal(DateTime.Today),
                    Arg<String>.Is.Null))
                .Return(new StatsResponse() {
                    statusCode = 200,
                    results = new List<StatsResponseResult> {
                        result
                    }
                });
            var startDate = DateTime.Today.AddDays(-1);
            var endDate = DateTime.Today;
            var actual = StatsClient.RetrieveStats(startDate, endDate, null);
            Assert.AreEqual(1, actual[0].Clicks);
            Assert.AreEqual(2, actual[0].Errors);
            Assert.AreEqual(3, actual[0].Opens);
            Assert.AreEqual(4, actual[0].Sent);
            Assert.AreEqual(5, actual[0].UniqueOpens);
        }

        /// <summary>
        ///A test for RetrieveUnsubscribes
        ///</summary>
        [TestMethod()]
        public void RetrieveUnsubscribesTest() {
            var result = new UnsubscribeResponseResult() {
                time = DateTime.Now,
                toEmail = "test@example.com"
            };
            MockHttpClient.Expect(
                x =>
                x.RetrieveUnsubscribes(
                    Arg<DateTime>.Is.Equal(DateTime.Today.AddDays(-1)),
                    Arg<DateTime>.Is.Equal(DateTime.Today)))
                .Return(new UnsubscribesResponse() {
                    statusCode = 200,
                    results = new List<UnsubscribeResponseResult> {
                        result
                    }
                });
            var startDate = DateTime.Today.AddDays(-1);
            var endDate = DateTime.Today;
            var actual = StatsClient.RetrieveUnsubscribes(startDate, endDate);
            Assert.AreEqual("test@example.com", actual[0].ToEmail);
        }
    }
}
