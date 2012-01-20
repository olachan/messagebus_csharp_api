// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System;
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

        private const string DemoTestApiKey = "<YOUR API KEY>";
        private const string DemoUrl = "https://api.messagebus.com";

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

        [TestMethod, Ignore]
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

        [TestMethod, Ignore]
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

        [TestMethod, Ignore]
        public void Sends40BlackHoleMessagesToDemoUsingATemplate() {
            var mb = MessageBus.API.MessageBusFactory.CreateEmailClient(DemoTestApiKey, new ConsoleLogger());
            mb.Transmitted += Transmitted;
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

        private static void Transmitted(IMessageBusTransmissionEvent evt) {
            Console.WriteLine("Messages Sent: {0}/{1}", evt.SuccessCount, evt.FailureCount);
            foreach (var s in evt.Statuses) {
                Console.WriteLine("MessageId {0}, Email {1}, Code {2}, Succeeded {3}", s.MessageId, s.ToEmail, s.Status, s.Succeeded);
            }
        }

        [TestMethod, Ignore]
        public void RetrievesStatsFromDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateStatsClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            var results = mb.RetrieveStats(null, null, null);
            Assert.IsNotNull(results);
        }

        [TestMethod, Ignore]
        public void RetrievesDeliveryErrorsFromDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateStatsClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            var results = mb.RetrieveDeliveryErrors(null, null, null);
            Assert.IsNotNull(results);
        }

        [TestMethod, Ignore]
        public void RetrievesUnsubscribesFromDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateStatsClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            var results = mb.RetrieveUnsubscribes(null, null);
            Assert.IsNotNull(results);
        }

        [TestMethod, Ignore]
        public void ListMailingListsFromDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateMailingListClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            var results = mb.ListMailingLists();
            Assert.IsNotNull(results);
        }

        [TestMethod, Ignore]
        public void CreateAMailingListOnDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateMailingListClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);
            var results = mb.CreateMailingList(new MessageBusMailingList { MergeFieldKeys = new[] { "%EMAIL%", "%NAME%" }, Name = "Test" });
            Assert.IsNotNull(results.Key);
        }

        [TestMethod, Ignore]
        public void CreateAMailingListEntryOnDemo() {
            var mb = MessageBus.API.MessageBusFactory.CreateMailingListClient(DemoTestApiKey, new ConsoleLogger());
            SetDebugOptions(mb);

            var list = mb.CreateMailingList(new MessageBusMailingList { MergeFieldKeys = new[] { "%EMAIL%", "%NAME%" }, Name = "Test" });
            var entry = new MessageBusMailingListEntry();
            entry.MergeFields.Add("%EMAIL%", "test@example.com");
            entry.MergeFields.Add("%NAME%", "test");
            mb.CreateMailingListEntry(list.Key, entry);
        }

        [TestMethod, Ignore]
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

        [TestMethod, Ignore]
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
