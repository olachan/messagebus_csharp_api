// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

// This example retrieves message delivery errors

using System;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleGetDeliveryErrors {

        // replace with YOUR PRIVATE key, which can be found here: https://www.messagebus.com/api
        private readonly IMessageBusStatsClient MessageBus = MessageBusFactory.CreateStatsClient("<YOUR API KEY>");

        /// <summary>
        /// GetDeliveryErrors optionally accepts startDate and endDate parameters which define the range of dates to
        /// supply stats for.  if these parameters are not supplied, startDate defaults to 7 days ago and 
        /// endDate defaults to today.  Do not enter a startDate greater than 7 days ago.
        /// </summary>
        void GetDeliveryErrors() {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(-1);

            // GetDeliveryErrors returns an array of items, each reflecting delivery errors for a single day within
            // the requested range.  Returned items are placed into list.
            MessageBusDeliveryErrorResult[] list;
            try {
                list = MessageBus.RetrieveDeliveryErrors(startDate, endDate);
            } catch (MessageBusException) {
                throw;
            }

            // Iterate over each item within list to see the results
            foreach (var item in list) {
                Console.WriteLine(String.Format("At {0} message {1} to {2} returned {3}", item.Time.ToString("o"), item.MessageId, item.ToEmail, item.DSNCode));
            }
        }

        /// <summary>
        /// GetDeliveryErrors optionally accepts startDate, endDate, and tag parameters which define the range of dates to
        /// supply stats for and filter by tag.  If these parameters are not supplied, startDate defaults to 7 days ago and 
        /// endDate defaults to today.  Do not enter a startDate greater than 7 days ago.
        /// </summary>
        void GetDeliveryErrorsWithTag()
        {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(-1);
            var tag = "csharp_example";

            // GetDeliveryErrors returns an array of items, each reflecting delivery errors for a single day within
            // the requested range.  Returned items are placed into list.
            MessageBusDeliveryErrorResult[] list;
            try
            {
                list = MessageBus.RetrieveDeliveryErrors(startDate, endDate, tag);
            }
            catch (MessageBusException)
            {
                throw;
            }

            // Iterate over each item within list to see the results
            foreach (var item in list)
            {
                var tags = String.Join(",", item.tags);
                Console.WriteLine(String.Format("At {0} message {1} to {2} with tags {3} returned {4}", item.Time.ToString("o"), item.MessageId, item.ToEmail, tags, item.DSNCode));
            }
        }

    }
}