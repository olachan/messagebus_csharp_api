// Copyright (c) 2011. Message Bus
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

// This example retrieves message statistics

using System;
using System.Collections.Generic;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleGetStats {

        // replace with YOUR PRIVATE key, which can be found here: https://www.messagebus.com/api
        private readonly IMessageBusStatsClient MessageBus = MessageBusFactory.CreateStatsClient("<YOUR API KEY>");

        /// <summary>
        /// GetStats optionally accepts startDate and endDate parameters which define the range of dates to
        /// supply stats for.  If these parameters are not supplied, startDate defaults to 30 days ago and
        /// endDate defaults to today.  Do not enter a startDate greater than 30 days ago.
        /// </summary>
        void GetStats() {
            var startDate = DateTime.Today.AddDays(-7);
            var endDate = DateTime.Today.AddDays(-1);

            // GetStats returns an array of items, each reflecting sending activity statistics for one day within
            // the requested range.  Returned items are placed into list.
             MessageBusStatsResult[] list;
            try {
                list = MessageBus.RetrieveStats(startDate, endDate, null);
            } catch (MessageBusException) {
                throw;
            }

            // Iterate over each item within list to see the results
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