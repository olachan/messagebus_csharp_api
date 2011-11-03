// This example sends multiple templated emails in a single https transaction to increase message throughput

using System;
using System.Collections.Generic;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleSendTemplateMessageDetailed {

        // If you are sending messages from various points across your code, the
        // MessageBusFactory will create multiple thread-safe object instances to
        // efficiently batch transactions, resulting in higher throughput.

        // replace with YOUR PRIVATE key, which can be found here: https://www.messagebus.com/api
        private readonly IMessageBusEmailClient MessageBus = MessageBusFactory.CreateEmailClient("<YOUR API KEY>");

        // hook up the event handler
        public ExampleSendTemplateMessageDetailed() {
            MessageBus.Transmitted += Transmitted;
        }

        // define one or more template message param arrays.  merge fields and custom headers
        // are appended to each message after construction
        void SendExampleTemplates() {

            var msg1 = new MessageBusTemplateEmail {
                ToEmail = "recipient1@example.com",
                ToName = "recipient1",
                TemplateKey = ""
            };
            msg1.MergeFields["%FIELD1"] = "value1";
            msg1.MergeFields["%FIELD2"] = "value2";
            msg1.CustomHeaders["HEADER1"] = "header1";
            msg1.CustomHeaders["HEADER2"] = "header2";

            var msg2 = new MessageBusTemplateEmail {
                ToEmail = "recipient2@example.com",
                ToName = "recipient2",
                TemplateKey = ""
            };
            msg2.MergeFields["%FIELD1"] = "value1";
            msg2.MergeFields["%FIELD2"] = "value2";
            msg2.CustomHeaders["HEADER1"] = "header1";
            msg2.CustomHeaders["HEADER2"] = "header2";

            var templates = new[] { msg1, msg2 };
            SendMessages(templates);
        }

        // The MessageBus API buffers email in a local queue to increase performance.  When size of the local queue
        // reaches a threshold (default is 20), the messages are automatically flushed and sent.  Remaining queued
        // messages are sent when the API instance is closed or destructed.  In the example below, the 'using' statement
        // provides a scope around the instance.  The instance exits this scope upon completion of the 'foreach' loop,
        // resulting in the flushing and subsequent destruction of the instance, and the sending of the messages.
        void SendMessages(IEnumerable<MessageBusTemplateEmail> emails) {
            using (MessageBus) {
                foreach (var email in emails) {
                    MessageBus.Send(email);
                }
            }
        }

        // SendMessages returns an array of items, including status information for the message batch, and
        // an array of individual status information for each message sent.  Returned information is placed
        // into e.  Information about individual messages is contained within e.Statuses
        static void Transmitted(IMessageBusTransmissionEvent e) {
            Console.WriteLine(String.Format("Email Delivered.  Succeeded:{0};  Failed:{1}", e.SuccessCount, e.FailureCount));
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