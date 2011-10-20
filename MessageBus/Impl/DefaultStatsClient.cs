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

        public MessageBusStatsResult[] RetrieveStats(DateTime? startDate, DateTime? endDate, string tag) {
            var response = HttpClient.RetrieveStats(startDate, endDate, tag);
            if (response.statusCode != 200) {
                throw new MessageBusException(response.statusCode, response.statusMessage);
            }
            return response.results.Select(r => new MessageBusStatsResult(r)).ToArray();
        }

        public MessageBusDeliveryErrorResult[] RetrieveDeliveryErrors(DateTime? startDate, DateTime? endDate)
        {
            var response = HttpClient.RetrieveDeliveryErrors(startDate, endDate);
            if (response.statusCode != 200) {
                throw new MessageBusException(response.statusCode, response.statusMessage);
            }
            return response.results.Select(r => new MessageBusDeliveryErrorResult(r)).ToArray();
        }

        public MessageBusUnsubscribeResult[] RetrieveUnsubscribes(DateTime? startDate, DateTime? endDate)
        {
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