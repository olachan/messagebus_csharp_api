// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System.Collections.Generic;

namespace MessageBus.API.V3 {
    public sealed class MessageBusCampaign {
        public MessageBusCampaign() {
            CustomHeaders = new Dictionary<string, string>();
        }
        public string CampaignName { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string MailingListKey { get; set; }
        public string HtmlBody { get; set; }
        public string PlaintextBody { get; set; }
        public string[] Tags { get; set; }
        public Dictionary<string, string> CustomHeaders { get; private set; }
    }
}