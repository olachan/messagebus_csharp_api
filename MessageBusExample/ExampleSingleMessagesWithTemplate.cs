using System;
using System.Collections.Generic;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleSingleMessagesWithTemplate {

        private readonly IMessageBusEmailClient MessageBus
            = MessageBusFactory.CreateEmailClient("<YOUR API KEY>");

        public ExampleSingleMessagesWithTemplate() {
            MessageBus.Transmitted += Transmitted;
            MessageBus.EmailBufferSize = 0;
        }

        void SendMessage(Dictionary<string, string> fields) {
            var email = new MessageBusTemplateEmail();
            foreach (var field in fields) {
                email.MergeFields[field.Key] = field.Value;
            }
            MessageBus.Send(email);
        }

        static void Transmitted(IMessageBusTransmissionEvent e) {
            Console.WriteLine(String.Format("Email Delivered.  Succeeded:{0};  Failed:{1}", e.SuccessCount, e.FailureCount));
        }
    }
}