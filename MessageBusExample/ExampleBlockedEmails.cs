using System;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleBlockedEmails {

        private readonly IMessageBusStatsClient MessageBus
            = MessageBusFactory.CreateStatsClient("<YOUR API KEY>");

        void GetUnsubscribes() {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(-1);

            var list = MessageBus.RetrieveUnsubscribes(startDate, endDate);

            Console.WriteLine(String.Format("Unsibscribe Results Retrieved.  Record Count:{0}", list.Length));
        }
    }
}