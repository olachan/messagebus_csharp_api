// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

using System;
using System.Net;

namespace MessageBus.API.V3 {
    public class MessageBusException : Exception {
        private readonly int _statusCode;
        private readonly string _statusMessage;

        public MessageBusException(int statusCode, string statusMessage)
            : base(String.Format("Communication Failed with error code: {0} - {1}", statusCode, statusMessage)) {
            _statusCode = statusCode;
            _statusMessage = statusMessage;
        }

        public MessageBusException(WebException e)
            : base(String.Format("Communication with Server Failed with error code: {0}", e.Status)) {
            _statusCode = -1;
            _statusMessage = e.Status.ToString();
        }

        public int StatusCode {
            get { return _statusCode; }
        }

        public string StatusMessage {
            get { return _statusMessage; }
        }

        public bool IsRetryable() {
            return _statusCode > 500 && _statusCode < 600;
        }
    }
}