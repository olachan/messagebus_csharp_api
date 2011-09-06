using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MessageBus.API;
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
    }
}
