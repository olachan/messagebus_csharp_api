using System;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleMultipleMessages {

        private readonly IMessageBusEmailClient MessageBus
            = MessageBusFactory.CreateEmailClient("<YOUR API KEY>");

        public ExampleMultipleMessages() {
            MessageBus.Transmitted += transmitted;
        }

        void SendMessages(string[] emailAdresses) {
            using (MessageBus) {
                foreach (string emailAddress in emailAdresses) {
                    var email = new MessageBusEmail {
                        ToEmail = emailAddress,
                        ToName = "",
                        FromEmail = "bob@example.com",
                        FromName = "",
                        Subject = "Example Email",
                        PlaintextBody = "This message is only a test",
                        Tags = new[] { "tag1", "tag2" }
                    };
                    MessageBus.Send(email);
                }
            }
        }

        void transmitted(IMessageBusTransmissionEvent e) {
            Console.WriteLine(String.Format("Email Delivered.  Succeeded:{0};  Failed:{1}", e.SuccessCount, e.FailureCount));
        }
    }
}