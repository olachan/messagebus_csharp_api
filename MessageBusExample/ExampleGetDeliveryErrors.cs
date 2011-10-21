using System;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleGetDeliveryErrors {

        private readonly IMessageBusStatsClient MessageBus = MessageBusFactory.CreateStatsClient("<YOUR API KEY>");

        void GetDeliveryErrors() {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(-1);

            MessageBusDeliveryErrorResult[] list;
            try {
                list = MessageBus.RetrieveDeliveryErrors(startDate, endDate);
            } catch (MessageBusException e) {
                throw;
            }

            foreach (var item in list) {
                Console.WriteLine(String.Format("At {0} message {1} to {2} returned {3}", item.Time.ToString("o"), item.MessageId, item.ToEmail, item.DSNCode));
            }
        }
    }
}