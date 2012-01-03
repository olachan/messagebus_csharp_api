// Copyright (c) 2011. Message Bus
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
    /// <summary>
    /// Used internally to format messages in JSON for transmission on the wire.
    /// </summary>
    public sealed class BatchEmailMessage {
        public BatchEmailMessage() {
            // required no-arg constructor - used for testing
            customHeaders = new Dictionary<string, string>(0);
        }

        public BatchEmailMessage(MessageBusEmail email) {
            toEmail = email.ToEmail;
            toName = email.ToName;
            fromEmail = email.FromEmail;
            fromName = email.FromName;
            subject = email.Subject;
            plaintextBody = email.PlaintextBody;
            htmlBody = email.HtmlBody;
            customHeaders = email.CustomHeaders;
            tags = email.Tags;
        }

        public string toEmail { get; set; }
        public string fromEmail { get; set; }
        public string toName { get; set; }
        public string fromName { get; set; }
        public string subject { get; set; }
        public string plaintextBody { get; set; }
        public string htmlBody { get; set; }
        public Dictionary<string, string> customHeaders { get; private set; }
        public string[] tags { get; set; }
        
    }
}