using System;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleSendMessageSimple {

        private readonly IMessageBusEmailClient MessageBus
            = MessageBusFactory.CreateEmailClient("<YOUR API KEY>");

        public ExampleSendMessageSimple() {
            MessageBus.Transmitted += Transmitted;
            MessageBus.EmailBufferSize = 0;
        }

        void SendMessage(string emailAddress, string name) {
            var email = new MessageBusEmail {
                ToEmail = emailAddress,
                FromEmail = "bob@example.com",
                Subject = "Single Message Sample",
                HtmlBody = "<html><body>This message is a test sent by the C# MessageBus client library.</body></html>"
            };
            MessageBus.Send(email);
        }

        static void Transmitted(IMessageBusTransmissionEvent e) {
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