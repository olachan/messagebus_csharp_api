// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System.Collections.Generic;
using MessageBus.API.V3;

namespace MessageBus.SPI {
    /// <summary>
    /// Used internally to format messages in JSON for transmission on the wire.
    /// </summary>
    public sealed class BatchTemplateMessage {
        public BatchTemplateMessage() {
            // required no-arg constructor - used for testing
        }

        public BatchTemplateMessage(MessageBusTemplateEmail email) {
            mergeFields = email.MergeFields;
            toEmail = email.ToEmail;
            toName = email.ToName;
            customHeaders = email.CustomHeaders;
            templateKey = email.TemplateKey;
        }

        public string toEmail { get; set; }
        public string templateKey { get; set; }
        public string toName { get; set; }
        public Dictionary<string, string> mergeFields { get; private set; }
        public Dictionary<string, string> customHeaders { get; private set; }
    }
}