// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

// NOTE: This example is temporarily disabled due to the pending replacement of CreateMailingList() 

// This example demonstrates various api methods relating to mailing list management.
// We create a new blank mailing list; add two entries to the new list; delete one of
// the entries; and then retrieve the list of all mailing lists

using System;
using System.IO;
using MessageBus.API;
using MessageBus.API.V3;
using MessageBus.Impl;

namespace MessageBusExample {

    public class ExampleMailingListUploadAndCampaignSend {

        // replace with YOUR PRIVATE key, which can be found here: https://www.messagebus.com/api
        private readonly IMessageBusMailingListClient MessageBusMailingLists = MessageBusFactory.CreateMailingListClient("<YOUR API KEY>", new ConsoleLogger());
        private readonly IMessageBusCampaignsClient MessageBusCampaigns = MessageBusFactory.CreateCampaignClient("<YOUR API KEY>", new ConsoleLogger());

        /// <summary>
        /// This example uploads a mailing list and sends a campaign based on the mailing list
        /// </summary>
        string RunExample(string name, FileInfo mailingList) {

            try {

                var uploadResult = MessageBusMailingLists.UploadMailingList(name, mailingList);

                var mailingListKey = uploadResult.Key;

                var request = new MessageBusCampaign() {
                    CampaignName = name,
                    Subject = "This is a Test Email",
                    FromName = "Bob",
                    FromEmail = "bob@example.com",
                    MailingListKey = mailingListKey,
                    HtmlBody = "<html><body>This is the HTML body</body></html>",
                    PlaintextBody = "This is the palintext body",
                };
                var campaignResult = MessageBusCampaigns.SendCampaign(request);

                return campaignResult.CampaignKey;

            } catch (MessageBusException) {
                throw;
            }
        }
    }
}