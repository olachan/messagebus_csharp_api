// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System;
using System.IO;
using System.Linq;
using System.Net;
using MessageBus.API;
using MessageBus.API.V3;
using MessageBus.API.V3.Debug;
using MessageBus.SPI;

namespace MessageBus.Impl {
    public class DefaultCampaignClient : IMessageBusCampaignsClient, IMessageBusDebugging {

        private readonly ILogger Logger;
        private readonly IMessageBusHttpClient HttpClient;

        public DefaultCampaignClient(string apiKey) {
            HttpClient = new SimpleHttpClient(apiKey);
            Logger = new NullLogger();
        }

        public DefaultCampaignClient(string apiKey, ILogger logger) {
            HttpClient = new SimpleHttpClient(apiKey);
            Logger = logger;
        }

        public DefaultCampaignClient(IMessageBusHttpClient httpClient, ILogger logger) {
            HttpClient = httpClient;
            Logger = logger;
        }

        public MessageBusCampaignResult SendCampaign(MessageBusCampaign request) {
            var response = HttpClient.SendCampaign(new CampaignSendRequest(request));
            if (response.statusCode != 202) {
                throw new MessageBusException(response.statusCode, response.statusMessage);
            }
            return new MessageBusCampaignResult(response);
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