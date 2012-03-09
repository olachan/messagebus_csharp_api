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
using MessageBus.API.V3;

namespace MessageBus.SPI {
    public class CampaignSendRequest {
        public CampaignSendRequest() {
            customHeaders = new Dictionary<string, string>();
        }
        public CampaignSendRequest(MessageBusCampaign request) {
            campaignName = request.CampaignName;
            fromName = request.FromName;
            fromEmail = request.FromEmail;
            subject = request.Subject;
            mailingListKey = request.MailingListKey;
            htmlBody = request.HtmlBody;
            plaintextBody = request.PlaintextBody;
            tags = request.Tags;
            customHeaders = request.CustomHeaders;
        }

        public string campaignName { get; set; }
        public string fromName { get; set; }
        public string fromEmail { get; set; }
        public string subject { get; set; }
        public string mailingListKey { get; set; }
        public string htmlBody { get; set; }
        public string plaintextBody { get; set; }
        public string[] tags { get; set; }
        public Dictionary<string, string> customHeaders { get; private set; }
    }
}