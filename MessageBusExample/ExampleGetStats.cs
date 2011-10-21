using System;
using System.Collections.Generic;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleGetStats {

        private readonly IMessageBusStatsClient MessageBus = MessageBusFactory.CreateStatsClient("<YOUR API KEY>");

        void GetDeliveryErrors() {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(-1);

            MessageBusStatsResult[] list;
            try {
                list = MessageBus.RetrieveStats(startDate, endDate, null);
            } catch (MessageBusException e) {
                throw;
            }

            foreach (var item in list) {
                Console.WriteLine(String.Format("At {0} there were {1} messages sent, {2} errors, {3} opens, {4} unique opens, {5} clicks."
                    , item.Date.ToString("o")
                    , item.Sent
                    , item.Errors
                    , item.Opens
                    , item.UniqueOpens
                    , item.Clicks));
            }
        }
    }
}