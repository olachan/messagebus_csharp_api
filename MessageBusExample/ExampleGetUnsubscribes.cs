// This example retrieves message unsubscribes

using System;
using System.Collections.Generic;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleGetUnsubscribes {

        // replace with YOUR PRIVATE key, which can be found here: https://www.messagebus.com/api
        private readonly IMessageBusStatsClient MessageBus = MessageBusFactory.CreateStatsClient("<YOUR API KEY>");

        /// <summary>
        /// GetUnsubscribes optionally accepts startDate and endDate parameters which define the range of dates to
        /// supply unsubscribes for.  if these parameters are not supplied, startDate defaults to 7 days ago and
        /// endDate defaults to today.  Do not enter a startDate greater than 7 days ago. 
        /// </summary>        
        void GetUnsubscribes() {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(-1);

            // GetUnsubscribes returns an array of items, each reflecting an unsubscribe request which occurred
            // within the requested range.  Returned items are placed into list.
            MessageBusUnsubscribeResult[] list;
            try {
                list = MessageBus.RetrieveUnsubscribes(startDate, endDate);
            } catch (MessageBusException) {
                throw;
            }

            // Iterate over each item within list to see the results
            foreach (var item in list) {
                Console.WriteLine(String.Format("{0} unsubscribed at {1}", item.ToEmail, item.Time.ToString("o")));
            }
        }
    }
}