// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

// This example demonstrates the simplest way to send a message.

using System;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleSendMessageSimple {

        // replace with YOUR PRIVATE key, which can be found here: https://www.messagebus.com/api
        private readonly IMessageBusEmailClient MessageBus
            = MessageBusFactory.CreateEmailClient("<YOUR API KEY>");

        /// <summary>
        /// Setting the EmailBufferSize to 0 flushes the message buffer and sends the message immediately
        /// </summary>
        public ExampleSendMessageSimple() {
            MessageBus.Transmitted += Transmitted;
            MessageBus.EmailBufferSize = 0;
        }

        /// <summary>
        /// If you are sending messages from various points across your code, the
        /// MessageBusFactory will create multiple thread-safe object instances to
        /// efficiently batch transactions, resulting in higher throughput.
        /// </summary>
        /// <param name="emailAddress">email address</param>
        /// <param name="name">email name</param>
        void SendMessage(string emailAddress, string name) {
            var email = new MessageBusEmail {
                ToEmail = emailAddress,
                FromEmail = "bob@example.com",
                Subject = "Single Message Sample",
                HtmlBody = "<html><body>This message is a test sent by the C# MessageBus client library.</body></html>"
            };
            MessageBus.Send(email);
        }
        
        /// <summary>
        /// SendMessage returns an array of items, including status information for the message batch, and
        /// an array of individual status information for each message sent.  Returned information is placed
        /// into e.  Information about individual messages is contained within e.Statuses
        /// </summary>
        /// <param name="e">tranmission event</param>
        static void Transmitted(IMessageBusTransmissionEvent e) {
            // In this example, we loop over each row within e.Statuses to provide feedback for each message sent
            foreach (var status in e.Statuses) {
                if (status.Succeeded) {
                    Console.WriteLine(String.Format("Email queued for delivery to {0}.  MessageId = {1}", status.ToEmail, status.MessageId));
                } else {
                    Console.WriteLine(String.Format("Email NOT queued for delivery to {0}.  Reason = {1}", status.ToEmail, status.Status));
                }
            }
        }
    }
}