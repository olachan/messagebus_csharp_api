// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System;
using System.Collections.Generic;
using System.IO;
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
                mailingListKey = "TEST",
                name = "hello",
                mergeFields = new[] { "%EMAIL%", "%NAME%" },
                validCount = 2,
                invalidCount = 1
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
            Assert.AreEqual("TEST", actual[0].MailingListKey);
            Assert.AreEqual("%EMAIL%", actual[0].MergeFieldKeys[0]);
            Assert.AreEqual("%NAME%", actual[0].MergeFieldKeys[1]);
            Assert.AreEqual(2, actual[0].ValidCount);
            Assert.AreEqual(1, actual[0].InvalidCount);
        }

        [TestMethod]
        public void UploadMailingListTest() {
            MockHttpClient.Expect(
                x =>
                x.UploadMailingList(Arg<MailingListUploadRequest>.Is.Anything, Arg<MailingListUploadProgressHandler>.Is.Anything))
                .Return(new MailingListUploadResponse() {
                    statusCode = 201,
                    statusMessage = "",
                    invalidCount = 0,
                    validCount = 1,
                    mailingListKey = "ABCDEF",
                    invalidLines = new int[0],
                    statusTime = DateTime.Now
                });
            var actual = MailingListClient.UploadMailingList("test", new FileInfo("TestData/simple.csv"));

            Assert.AreEqual("ABCDEF", actual.MailingListKey);
            Assert.AreEqual(1, actual.ValidCount);
            Assert.AreEqual(0, actual.InvalidCount);
            Assert.AreEqual(0, actual.InvalidLineNumbers.Length);
        }

        [TestMethod]
        public void DeleteMailingListTest() {
            MockHttpClient.Expect(
                x =>
                x.UploadMailingList(Arg<MailingListUploadRequest>.Is.Anything, Arg<MailingListUploadProgressHandler>.Is.Anything))
                .Return(new MailingListUploadResponse() {
                    statusCode = 201,
                    statusMessage = "",
                    invalidCount = 0,
                    validCount = 1,
                    mailingListKey = "ABCDEF",
                    invalidLines = new int[0],
                    statusTime = DateTime.Now
                });
            var actual = MailingListClient.UploadMailingList("test", new FileInfo("TestData/simple.csv"));

            Assert.AreEqual("ABCDEF", actual.MailingListKey);
            Assert.AreEqual(1, actual.ValidCount);
            Assert.AreEqual(0, actual.InvalidCount);
            Assert.AreEqual(0, actual.InvalidLineNumbers.Length);


            MockHttpClient.Expect(
                x =>
                x.DeleteMailingList(Arg<string>.Is.Equal(actual.MailingListKey)))
                .Return(new MailingListDeleteResponse()
                {
                    statusCode = 200,
                    statusMessage="testMessage",
                    statusTime = DateTime.Now
                });
            MailingListClient.DeleteMailingList(actual.MailingListKey);
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
            MailingListClient.AddMailingListEntry("TEST", entry);
        }
    }
}
