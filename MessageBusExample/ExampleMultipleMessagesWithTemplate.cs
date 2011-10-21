using System;
using System.Collections.Generic;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleMultipleMessagesWithTemplate {

        private readonly IMessageBusEmailClient MessageBus
            = MessageBusFactory.CreateEmailClient("<YOUR API KEY>");

        public ExampleMultipleMessagesWithTemplate() {
            MessageBus.Transmitted += transmitted;
        }

        void SendMessages(Dictionary<string, string> fields) {
            using (MessageBus) {
                foreach (var field in fields) {
                    var email = new MessageBusTemplateEmail();
                    email.MergeFields[field.Key] = field.Value;
                    MessageBus.Send(email);
                }
            }
        }

        void transmitted(IMessageBusTransmissionEvent e) {
            Console.WriteLine(String.Format("Email Delivered.  Succeeded:{0};  Failed:{1}", e.SuccessCount, e.FailureCount));
        }
    }
}