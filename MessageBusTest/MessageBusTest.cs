using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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

        [TestMethod]
        public void CanCreateANewMessageBusClientWithAnApiKey() {
            var mb = MessageBus.API.MessageBus.CreateClient("ABCD-1234-1234-ABCD");
            Assert.IsNotNull(mb);
            Assert.AreEqual("ABCD-1234-1234-ABCD", mb.ApiKey);
        }

        [TestMethod]
        public void CanCreateANewMessageBusClientWithACustomLogger() {
            var mb = MessageBus.API.MessageBus.CreateClient("ABCD-1234-1234-ABCD", new ConsoleLogger());
            Assert.IsNotNull(mb);
        }

        [TestMethod]
        public void SendsABlackHoleMessageToDemo() {
            var mb = MessageBus.API.MessageBus.CreateClient("746296C8062E4CF82F69621850282BBF", new ConsoleLogger());
            SetDebugOptions(mb);
            using (mb) {
                var email = new MessageBusEmail
                                {
                                    ToEmail = "alice@example.com",
                                    FromEmail = "bob@example.com",
                                    Tags = new[] {"TAGA"},
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
            var mb = MessageBus.API.MessageBus.CreateClient("746296C8062E4CF82F69621850282BBF", new ConsoleLogger());
            SetDebugOptions(mb);
            using (mb) {
                var email = new MessageBusTemplateEmail
                                {
                                    ToEmail = "joe@example.com",
                                    ToName = "Joe Soap",
                                    TemplateKey = "24E26340A4E6012E8C2940406818E8C7"
                                };
                email.MergeFields.Add("%EMAIL%", "joe@example.com");
                email.MergeFields.Add("%NAME%", "Joe Soap");
                mb.Send(email);
            }
        }

        private void SetDebugOptions(IMessageBusClient client) {
            var debug = client as IMessageBusDebugging;
            debug.Domain = "https://api.demo.messagebus.com";
            debug.SslVerifyPeer = false;
            debug.Credentials = new NetworkCredential("<user>", "<password>");
        }
    }
}
