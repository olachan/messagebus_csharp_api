using System;
using System.Collections.Generic;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleSendTemplateMessageDetailed {

        private readonly IMessageBusEmailClient MessageBus = MessageBusFactory.CreateEmailClient("<YOUR API KEY>");

        public ExampleSendTemplateMessageDetailed() {
            MessageBus.Transmitted += Transmitted;
        }

        void SendMessages(IEnumerable<Dictionary<string, string>> emails) {
            using (MessageBus) {
                foreach (var fields in emails) {
                    var email = new MessageBusTemplateEmail();
                    foreach (var field in fields) {
                        email.MergeFields[field.Key] = field.Value;
                    }
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