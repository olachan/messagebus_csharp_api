// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MessageBus.API;
using MessageBus.API.V3;
using MessageBus.API.V3.Debug;
using MessageBus.SPI;

namespace MessageBus.Impl {
    public class DefaultStatsClient : IMessageBusStatsClient, IMessageBusDebugging {

        private readonly ILogger Logger;
        private readonly IMessageBusHttpClient HttpClient;

        public DefaultStatsClient(string apiKey) {
            HttpClient = new SimpleHttpClient(apiKey);
            Logger = new NullLogger();
        }

        public DefaultStatsClient(string apiKey, ILogger logger) {
            HttpClient = new SimpleHttpClient(apiKey);
            Logger = logger;
        }

        public DefaultStatsClient(IMessageBusHttpClient httpClient, ILogger logger) {
            HttpClient = httpClient;
            Logger = logger;
        }

        public MessageBusStatsResult[] RetrieveStats(DateTime? startDate, DateTime? endDate, string tag) {
            var response = HttpClient.RetrieveStats(startDate, endDate, tag);
            if (response.statusCode != 200) {
                throw new MessageBusException(response.statusCode, response.statusMessage);
            }
            return response.results.Select(r => new MessageBusStatsResult(r)).ToArray();
        }

        public MessageBusDeliveryErrorResult[] RetrieveDeliveryErrors(DateTime? startDate, DateTime? endDate)
        {
            return RetrieveDeliveryErrors(startDate, endDate, null);
        }

        public MessageBusDeliveryErrorResult[] RetrieveDeliveryErrors(DateTime? startDate, DateTime? endDate, string tag) {
            var response = HttpClient.RetrieveDeliveryErrors(startDate, endDate, tag);
            if (response.statusCode != 200) {
                throw new MessageBusException(response.statusCode, response.statusMessage);
            }
            return response.results.Select(r => new MessageBusDeliveryErrorResult(r)).ToArray();
        }

        public MessageBusUnsubscribeResult[] RetrieveUnsubscribes(DateTime? startDate, DateTime? endDate) {
            var response = HttpClient.RetrieveUnsubscribes(startDate, endDate);
            if (response.statusCode != 200) {
                throw new MessageBusException(response.statusCode, response.statusMessage);
            }
            return response.results.Select(r => new MessageBusUnsubscribeResult(r)).ToArray();
        }

        public bool SkipValidation { private get; set; }


        public string Domain {
            set { HttpClient.Domain = value; }
        }

        public string Path {
            set { HttpClient.Path = value; }
        }

        public bool SslVerifyPeer {
            set { HttpClient.SslVerifyPeer = value; }
        }

        public IWebProxy Proxy {
            set { HttpClient.Proxy = value; }
        }
    }
}