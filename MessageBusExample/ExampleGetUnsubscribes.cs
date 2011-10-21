using System;
using System.Collections.Generic;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleGetUnsubscribes {

        private readonly IMessageBusStatsClient MessageBus = MessageBusFactory.CreateStatsClient("<YOUR API KEY>");

        void GetDeliveryErrors() {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(-1);

            MessageBusUnsubscribeResult[] list;
            try {
                list = MessageBus.RetrieveUnsubscribes(startDate, endDate);
            } catch (MessageBusException e) {
                throw;
            }

            foreach (var item in list) {
                Console.WriteLine(String.Format("{0} unsubscribed at {1}", item.ToEmail, item.Time.ToString("o")));
            }
        }
    }
}