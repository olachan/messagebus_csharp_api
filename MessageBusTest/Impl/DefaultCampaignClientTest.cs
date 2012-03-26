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
    public class DefaultCampaignClientTest {

        private DefaultCampaignClient CampaignClient;
        private IMessageBusHttpClient MockHttpClient;
        private ILogger MockLogger;
        //private IMessageBusTransmissionEvent TestEvent;

        public TestContext TestContext { get; set; }

        [TestInitialize()]
        public void MyTestInitialize() {
            MockHttpClient = MockRepository.GenerateMock<IMessageBusHttpClient>();
            MockLogger = MockRepository.GenerateMock<ILogger>();
            CampaignClient = new DefaultCampaignClient(MockHttpClient, MockLogger);
        }

        [TestMethod()]
        public void SendCampaignTest() {
            MockHttpClient.Expect(
                x =>
                x.SendCampaign(Arg<CampaignSendRequest>.Is.Anything))
                .Return(new CampaignSendResponse() {
                    statusCode = 202,
                    statusMessage = "",
                    statusTime = DateTime.UtcNow,
                    campaignKey = "TEST1234"
                });
            var request = new MessageBusCampaign() {
                CampaignName = "Test",
                FromName = "Bob",
                FromEmail = "bob@example.com",
                Subject = "This is a test",
                MailingListKey = "avalidmailinglistkey",
                HtmlBody = "<html><body>This is the HTML body</body></html>",
                PlaintextBody = "This is the plaintext body",
                Tags = new[] { "TAGA TAGB" }
            };
            request.CustomHeaders.Add("X-Custom_Header", "somevalue");
            var actual = CampaignClient.SendCampaign(request);
            Assert.AreEqual("TEST1234", actual.CampaignKey);            
        }

        [TestMethod()]
        public void CampaignStatusTest()
        {
            MockHttpClient.Expect(
                 x =>
                 x.CampaignStatus(Arg<String>.Is.Anything))
                 .Return(new CampaignStatusResponse()
                 {
                     completed = true,
                     statusCode = 202,
                     statusMessage = "test campaign status message",
                     statusTime = DateTime.UtcNow,
                 }
                 );

            CampaignStatusResponse csr = MockHttpClient.CampaignStatus("TEST1234");
            Assert.AreEqual(202, csr.statusCode);
            Assert.AreEqual(true, csr.completed);
            Assert.AreEqual("test campaign status message", csr.statusMessage);
        }

        [TestMethod()]
        public void ListCampaignsTest()
        {
            var result = new CampaignsResponseResult()
            {
                campaignKey = "TEST1234",
                name = "hello",
                published = true,               
            };            
            
            MockHttpClient.Expect(
                x =>
                x.ListCampaigns())
                .Return(new CampaignsResponse()
                {
                    statusCode = 200,
                    statusMessage = "test campaign status message",
                    statusTime = DateTime.UtcNow,
                    count = 1,
                    results = new List<CampaignsResponseResult> {
                        result
                    }
                });

            CampaignsResponse response = MockHttpClient.ListCampaigns();
            Assert.AreEqual(200, response.statusCode);
            Assert.AreEqual(1, response.count);
            Assert.AreEqual("test campaign status message", response.statusMessage);            
            Assert.AreEqual("TEST1234", response.results[0].campaignKey);            
        }
    }
}
