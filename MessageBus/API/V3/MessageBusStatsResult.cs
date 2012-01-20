// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System;
using MessageBus.SPI;

namespace MessageBus.API.V3 {
    public class MessageBusStatsResult {
        public MessageBusStatsResult(StatsResponseResult result) {
            Date = result.date;
            Sent = result.sent;
            Errors = result.errors;
            Opens = result.opens;
            UniqueOpens = result.uniqueOpens;
            Clicks = result.clicks;
        }

        public DateTime Date { get; private set; }
        public int Sent { get; private set; }
        public int Errors { get; private set; }
        public int Opens { get; private set; }
        public int UniqueOpens { get; private set; }
        public int Clicks { get; private set; }
    }
}