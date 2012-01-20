// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using MessageBus.API.V3;
using MessageBus.Impl;

namespace MessageBus.API {
    /// <summary>
    /// MessageBus Factory class for simple, (non-inversion of control) clients
    /// </summary>
    public sealed class MessageBusFactory {

        public static IMessageBusEmailClient CreateEmailClient(string apiKey) {
            return new AutoBatchingEmailClient(apiKey);
        }

        public static IMessageBusEmailClient CreateEmailClient(string apiKey, ILogger logger) {
            return new AutoBatchingEmailClient(apiKey, logger);
        }

        public static IMessageBusStatsClient CreateStatsClient(string apiKey) {
            return new DefaultStatsClient(apiKey);
        }

        public static IMessageBusStatsClient CreateStatsClient(string apiKey, ILogger logger) {
            return new DefaultStatsClient(apiKey, logger);
        }

        public static IMessageBusMailingListClient CreateMailingListClient(string apiKey) {
            return new DefaultMailingListClient(apiKey);
        }

        public static IMessageBusMailingListClient CreateMailingListClient(string apiKey, ILogger logger) {
            return new DefaultMailingListClient(apiKey, logger);
        }

    }
}