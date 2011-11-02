// This example retrieves message delivery errors

using System;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleGetDeliveryErrors {

        // replace with YOUR PRIVATE key, which can be found here: https://www.messagebus.com/api
        private readonly IMessageBusStatsClient MessageBus = MessageBusFactory.CreateStatsClient("<YOUR API KEY>");

        // getDeliveryErrors optionally accepts startDate and endDate parameters which define the range of dates to
        // supply stats for.  if these parameters are not supplied, startDate defaults to 7 days ago and
        // endDate defaults to today.  Do not enter a startDate greater than 7 days ago.
        void GetDeliveryErrors() {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(-1);

            // getDeliveryErrors returns an array of items, each reflecting delivery errors for a single day within
            // the requested range.  Returned items are placed into list.
            MessageBusDeliveryErrorResult[] list;
            try {
                list = MessageBus.RetrieveDeliveryErrors(startDate, endDate);
            } catch (MessageBusException e) {
                throw;
            }

            // Iterate over each item within list to see the results
            foreach (var item in list) {
                Console.WriteLine(String.Format("At {0} message {1} to {2} returned {3}", item.Time.ToString("o"), item.MessageId, item.ToEmail, item.DSNCode));
            }
        }
    }
}