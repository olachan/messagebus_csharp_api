using System;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleErrorReport {

        private readonly IMessageBusStatsClient MessageBus
            = MessageBusFactory.CreateStatsClient("<YOUR API KEY>");

        void GetErrorReport() {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(-1);

            var list = MessageBus.RetrieveDeliveryErrors(startDate, endDate);

            Console.WriteLine(String.Format("Delevery Errors Retrieved.  Record Count:{0}", list.Length));
        }
    }
}