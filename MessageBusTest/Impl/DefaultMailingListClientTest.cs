// Copyright (c) 2011. Message Bus
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System.Collections.Generic;
using MessageBus.API;
using MessageBus.Impl;
using MessageBus.SPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBus.API.V3;
using Rhino.Mocks;

namespace MessageBusTest.Impl {
    [TestClass()]
    public class DefaultMailingListClientTest {

        private DefaultMailingListClient MailingListClient;
        private IMessageBusHttpClient MockHttpClient;
        private ILogger MockLogger;
        //private IMessageBusTransmissionEvent TestEvent;

        public TestContext TestContext { get; set; }

        [TestInitialize()]
        public void MyTestInitialize() {
            MockHttpClient = MockRepository.GenerateMock<IMessageBusHttpClient>();
            MockLogger = MockRepository.GenerateMock<ILogger>();
            MailingListClient = new DefaultMailingListClient(MockHttpClient, MockLogger);
        }

        [TestMethod()]
        public void ListMailingListsTest() {
            var result = new MailingListItem() {
                key = "TEST",
                name = "hello"
            };
            MockHttpClient.Expect(
                x =>
                x.ListMailingLists())
                .Return(new MailingListsResponse() {
                    statusCode = 200,
                    results = new List<MailingListItem> {
                        result
                    }
                });
            var actual = MailingListClient.ListMailingLists();
            Assert.AreEqual("hello", actual[0].Name);
            Assert.AreEqual("TEST", actual[0].Key);
        }

        [TestMethod()]
        public void DeleteMailingListEntryTest() {
            MockHttpClient.Expect(
                x =>
                x.DeleteMailingListEntry(Arg<string>.Is.Equal("TEST"), Arg<string>.Is.Equal("test@example.com")))
                .Return(new MailingListEntryDeleteResponse() {
                    statusCode = 200,
                });
            MailingListClient.DeleteMailingListEntry("TEST", "test@example.com");
        }

        [TestMethod()]
        public void CreateMailingListEntryTest() {
            MockHttpClient.Expect(
                x =>
                x.CreateMailingListEntry(Arg<string>.Is.Equal("TEST"), Arg<MailingListEntryCreateRequest>.Is.Anything))
                .Return(new MailingListEntryCreateResponse {
                    statusCode = 201,
                });
            var entry = new MessageBusMailingListEntry();
            entry.MergeFields["%EMAIL%"] = "test@example.com";
            MailingListClient.CreateMailingListEntry("TEST", entry);
        }

        [TestMethod()]
        public void CreateMailingListTest() {
            MockHttpClient.Expect(
                x =>
                x.CreateMailingList(Arg<MailingListCreateRequest>.Is.Anything))
                .Return(new MailingListCreateResponse {
                    statusCode = 201,
                    key = "TEST"
                });
            var actual = MailingListClient.CreateMailingList(new MessageBusMailingList {
                MergeFieldKeys = new[] { "%EMAIL", "%NAME%" },
                Name = "Test"
            });
            Assert.AreEqual("Test", actual.Name);
            Assert.AreEqual("TEST", actual.Key);
        }
    }
}
