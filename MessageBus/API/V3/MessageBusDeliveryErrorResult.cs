// Copyright (c) 2011. Message Bus
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System;
using MessageBus.SPI;

namespace MessageBus.API.V3 {
    public class MessageBusDeliveryErrorResult {
        public MessageBusDeliveryErrorResult(DeliveryErrorsResponseResult result) {
            ToEmail = result.toEmail;
            MessageId = result.messageId;
            Time = result.time;
            DSNCode = result.DSNCode;
        }

        public string ToEmail { get; private set; }
        public string MessageId { get; private set; }
        public DateTime Time { get; private set; }
        public string DSNCode { get; private set; }
    }
}