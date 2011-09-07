using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MessageBus.API;
using MessageBus.API.V2;
using MessageBus.API.V2.Debug;
using MessageBus.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageBusTest {
    /// <summary>
    /// Summary description for MessageBusTest
    /// </summary>
    [TestClass]
    public class MessageBusTest {

        [TestMethod]
        public void CanCreateANewMessageBusClientWithAnApiKey() {
            var mb = MessageBus.API.MessageBus.CreateClient("ABCD-1234-1234-ABCD");
            Assert.IsNotNull(mb);
            Assert.AreEqual("2.2", mb.ApiVersion);
        }

        [TestMethod]
        public void CanCreateANewMessageBusClientWithAnApiKeyAndAVersion() {
            var mb = MessageBus.API.MessageBus.CreateClient("ABCD-1234-1234-ABCD", 2);
            Assert.IsNotNull(mb);
            Assert.AreEqual("2.2", mb.ApiVersion);
        }

        [TestMethod]
        public void CanCreateANewMessageBusClientWithACustomLogger() {
            var mb = MessageBus.API.MessageBus.CreateClient("ABCD-1234-1234-ABCD", new ConsoleLogger());
            Assert.IsNotNull(mb);
            Assert.AreEqual("2.2", mb.ApiVersion);
        }

        [TestMethod]
        public void CanCreateANewMessageBusClientWithAVersionAndCustomLogger() {
            var mb = MessageBus.API.MessageBus.CreateClient("ABCD-1234-1234-ABCD", 2, new ConsoleLogger());
            Assert.IsNotNull(mb);
            Assert.AreEqual("2.2", mb.ApiVersion);
        }

        [TestMethod]
        public void SendsABlackHoleMessageToDemo() {
            var mb = MessageBus.API.MessageBus.CreateClient("746296C8062E4CF82F69621850282BBF", 2, new ConsoleLogger());
            SetDebugOptions(mb);
            mb.FromEmail = "bob@example.com";
            mb.Tags = new[] { "TAGA" };
            mb.CustomHeaders.Add("REPLY-TO", "do-not-reply@example.com");
            using (mb) {
                mb.Send(new MessageBusEmail {
                    ToEmail = "alice@example.com",
                    Subject = "Test from Messagebus",
                    PlaintextBody = "Some Body Text",
                    HtmlBody = "<html><body><h1>Hello!</h1></body></htm>"
                });
            }
        }

        [TestMethod]
        public void SendsABlackHoleMessageToDemoUsingATemplate() {
            var mb = MessageBus.API.MessageBus.CreateClient("746296C8062E4CF82F69621850282BBF", 2, new ConsoleLogger());
            SetDebugOptions(mb);
            mb.TemplateKey = "24E26340A4E6012E8C2940406818E8C7";
            using (mb) {
                var email = new MessageBusTemplateEmail();
                email.MergeFields.Add("%EMAIL%", "joe@example.com");
                email.MergeFields.Add("%NAME%", "Joe Soap");
                mb.Send(email);
            }
        }

        private void SetDebugOptions(IMessageBusClient client) {
            var debug = client as IMessageBusDebugging;
            debug.Domain = "https://apitest.messagebus.com";
            debug.SslVerifyPeer = false;
            debug.Credentials = new NetworkCredential("user", "password");
        }
    }
}
