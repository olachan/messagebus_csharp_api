using MessageBus.API;
using MessageBus.API.V3;
using MessageBus.API.V3.Debug;
using MessageBus.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageBusTest {
    /// <summary>
    /// Summary description for MessageBusTest
    /// </summary>
    [TestClass]
    public class MessageBusTest {

        private const string DemoTestApiKey = "746296C8062E4CF82F69621850282BBF";
        private const string DemoUrl = "https://api.demo.messagebus.com";

        [TestMethod]
        public void CanCreateANewMessageBusClientWithAnApiKey() {
            var mb = MessageBusFactory.CreateEmailClient("ABCD-1234-1234-ABCD");
            Assert.IsNotNull(mb);
            Assert.AreEqual("ABCD-1234-1234-ABCD", mb.ApiKey);
        }

        [TestMethod]
        public void CanCreateANewMessageBusClientWithACustomLogger() {
            var mb = MessageBusFactory.CreateEmailClient("ABCD-1234-1234-ABCD", new ConsoleLogger());
            Assert.IsNotNull(mb);
        }

        [TestMethod]
        public void SendsABlackHoleMessageToDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateEmailClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            using (mb) {
                var email = new MessageBusEmail {
                    ToEmail = "alice@blackhole.messagebus.com",
                    FromEmail = "bob@example.com",
                    Tags = new[] { "TAGA" },
                    Subject = "Test from Messagebus",
                    PlaintextBody = "Some Body Text",
                    HtmlBody = "<html><body><h1>Hello!</h1></body></htm>"
                };

                email.CustomHeaders.Add("REPLY-TO", "do-not-reply@example.com");

                mb.Send(email);
            }
        }

        [TestMethod]
        public void SendsABlackHoleMessageToDemoUsingATemplate() {
            var mb = MessageBus.API.MessageBusFactory.CreateEmailClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            using (mb) {
                var email = new MessageBusTemplateEmail {
                    ToEmail = "alice@blackhole.messagebus.com",
                    ToName = "Alice",
                    TemplateKey = "24E26340A4E6012E8C2940406818E8C7"
                };
                email.MergeFields.Add("%EMAIL%", "joe@example.com");
                email.MergeFields.Add("%NAME%", "Joe Soap");
                mb.Send(email);
            }
        }

        [TestMethod]
        public void Sends40BlackHoleMessagesToDemoUsingATemplate() {
            var mb = MessageBus.API.MessageBusFactory.CreateEmailClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            using (mb) {
                for (int i = 0; i < 40; i++) {
                    var email = new MessageBusTemplateEmail {
                        ToEmail = string.Format("alice-{0}@blackhole.messagebus.com", i),
                        ToName = "Alice",
                        TemplateKey = "24E26340A4E6012E8C2940406818E8C7"
                    };
                    email.MergeFields.Add("%EMAIL%", string.Format("alice-{0}@blackhole.messagebus.com", i));
                    email.MergeFields.Add("%NAME%", "Alice");
                    mb.Send(email);
                }
            }
        }

        [TestMethod]
        public void RetrievesStatsFromDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateStatsClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            var results = mb.RetrieveStats(null, null, null);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void RetrievesDeliveryErrorsFromDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateStatsClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            var results = mb.RetrieveDeliveryErrors(null, null);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void RetrievesUnsubscribesFromDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateStatsClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            var results = mb.RetrieveUnsubscribes(null, null);
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void ListMailingListsFromDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateMailingListClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            var results = mb.ListMailingLists();
            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void CreateAMailingListOnDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateMailingListClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            var results = mb.CreateMailingList(new MessageBusMailingList { MergeFieldKeys = new[] { "%EMAIL%", "%NAME%" }, Name = "Test" });
            Assert.IsNotNull(results.Key);
        }

        [TestMethod]
        public void CreateAMailingListEntryOnDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateMailingListClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);

            var list = mb.CreateMailingList(new MessageBusMailingList { MergeFieldKeys = new[] { "%EMAIL%", "%NAME%" }, Name = "Test" });
            var entry = new MessageBusMailingListEntry();
            entry.MergeFields.Add("%EMAIL%", "test@example.com");
            entry.MergeFields.Add("%NAME%", "test");
            mb.CreateMailingListEntry(list.Key, entry);
        }

        [TestMethod]
        public void DeleteAMailingListEntryOnDemo() {
            var mb = MessageBusFactory.CreateMailingListClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            var list = mb.CreateMailingList(new MessageBusMailingList { MergeFieldKeys = new[] { "%EMAIL%", "%NAME%" }, Name = "Test" });
            var entry = new MessageBusMailingListEntry();
            entry.MergeFields.Add("%EMAIL%", "test@example.com");
            entry.MergeFields.Add("%NAME%", "test");
            mb.CreateMailingListEntry(list.Key, entry);
            mb.DeleteMailingListEntry(list.Key, "test@example.com");
        }

        [TestMethod]
        public void HandlesExceptionsCorrectly() {
            var mb = MessageBus.API.MessageBusFactory.CreateMailingListClient("IAMANINVALIDKEYBLAR", new ConsoleLogger());
            SetDebugOptions(mb);
            try {
                mb.ListMailingLists();
            } catch (MessageBusException e) {
                Assert.AreEqual(403, e.StatusCode);
                return;
            }
            Assert.Fail("MessageBus Exception was not thrown");
        }

        private void SetDebugOptions(IMessageBusDebugging debug) {
            debug.Domain = DemoUrl;
            debug.SslVerifyPeer = false;
        }
        private void SetDebugOptions(IMessageBusStatsClient emailClient) {
            SetDebugOptions(emailClient as IMessageBusDebugging);
        }
        private void SetDebugOptions(IMessageBusEmailClient emailClient) {
            SetDebugOptions(emailClient as IMessageBusDebugging);
        }
        private void SetDebugOptions(IMessageBusMailingListClient emailClient) {
            SetDebugOptions(emailClient as IMessageBusDebugging);
        }
    }
}
