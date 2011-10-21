using System;
using System.Collections.Generic;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleSendMessageDetailed {

        private readonly IMessageBusEmailClient MessageBus
            = MessageBusFactory.CreateEmailClient("<YOUR API KEY>");

        public ExampleSendMessageDetailed() {
            MessageBus.Transmitted += Transmitted;
        }

        void SendMessages(IEnumerable<string> emailAdresses) {
            using (MessageBus) {
                foreach (string emailAddress in emailAdresses) {
                    var email = new MessageBusEmail {
                        ToEmail = emailAddress,
                        ToName = "",
                        FromEmail = "bob@example.com",
                        FromName = "Bob",
                        Subject = "Test API Email",
                        HtmlBody = "<html><body>This is a test message from the MessageBus C# client.</body></html>",
                        PlaintextBody = "This is a test message from the MessageBus C# client",
                        Tags = new[] { "tag1", "tag2" }
                    };
                    email.CustomHeaders["HEADER"] = "example header";
                    MessageBus.Send(email);
                }
            }
        }

        static void Transmitted(IMessageBusTransmissionEvent e) {
            Console.WriteLine(String.Format("Email Delivered.  Succeeded:{0};  Failed:{1}", e.SuccessCount, e.FailureCount));
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