// This example sends multiple emails in a single https transaction to increase message throughput.

using System;
using System.Collections.Generic;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleSendMessageDetailed {

        // replace with YOUR PRIVATE key, which can be found here: https://www.messagebus.com/api
        private readonly IMessageBusEmailClient MessageBus
            = MessageBusFactory.CreateEmailClient("<YOUR API KEY>");

        /// <summary>
        /// Hook up the event handler
        /// </summary>
        public ExampleSendMessageDetailed() {
            MessageBus.Transmitted += Transmitted;
        }

        /// <summary>
        /// If you are sending messages from various points across your code, the
        /// MessageBusFactory will create multiple thread-safe object instances to
        /// efficiently batch transactions, resulting in higher throughput.
        /// </summary>
        void SendExampleEmails() {

            var msg1 = new MessageBusEmail {
                ToEmail = "recipient1@example.com",
                ToName = "recipient1",
                FromEmail = "bob@example.com",
                FromName = "Bob",
                Subject = "Test API Email",
                HtmlBody =
                    "<html><body>This is a test message from the MessageBus C# client.</body></html>",
                PlaintextBody = "This is a test message from the MessageBus C# client",
                Tags = new[] { "tag1", "tag2" }
            };
            msg1.CustomHeaders["HEADER"] = "example header";

            var msg2 = new MessageBusEmail {
                ToEmail = "recipient2@example.com",
                ToName = "recipient2",
                FromEmail = "bob@example.com",
                FromName = "Bob",
                Subject = "Test API Email with a different subject",
                HtmlBody =
                    "<html><body>This is a test message from the MessageBus C# client with a different HTML body.</body></html>",
                PlaintextBody =
                    "This is a test message from the MessageBus C# client with a different Plaintext body",
                Tags = new[] { "tag3", "tag4" }
            };
            msg2.CustomHeaders["HEADER"] = "example header";

            var emails = new[] {
              msg1, msg2  
            };
            SendMessages(emails);
        }


        /// <summary>
        /// The MessageBus API buffers email in a local queue to increase performance.  When size of the local queue
        /// reaches a threshold (default is 20), the messages are automatically flushed and sent.  Remaining queued
        /// messages are sent when the API instance is closed or destructed.  In the example below, the 'using' statement
        /// provides a scope around the instance.  The instance exits this scope upon completion of the 'foreach' loop,
        /// resulting in the flushing and subsequent destruction of the instance, and the sending of the messages.
        /// </summary>
        /// <param name="emails">array of messages</param>
       
        void SendMessages(IEnumerable<MessageBusEmail> emails) {
            using (MessageBus) {
                foreach (var email in emails) {
                    MessageBus.Send(email);
                }
            }
        }

        /// <summary>
        /// SendMessages returns an array of items, including status information for the message batch, and
        /// an array of individual status information for each message sent.  Returned information is placed
        /// into e.  Information about individual messages is contained within e.Statuses
        /// </summary>
        /// <param name="e">transmission event</param>
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