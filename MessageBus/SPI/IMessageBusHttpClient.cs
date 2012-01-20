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
using System.Web.Script.Serialization;

namespace MessageBus.SPI {
    /// <summary>
    /// Internal Service Provider interface to handle the physical transmission of messages over the wire.
    /// </summary>
    public interface IMessageBusHttpClient {
        JavaScriptSerializer Serializer { get; set; }
        string Domain { set; }
        string Path { set; }
        IWebProxy Proxy { set; }
        ICredentials Credentials { set; }
        bool SslVerifyPeer { set; }
        BatchEmailResponse SendEmails(BatchEmailSendRequest batchEmailSendRequest);
        BatchEmailResponse SendEmails(BatchTemplateSendRequest batchTemplateSendRequest);
        StatsResponse RetrieveStats(DateTime? startDate, DateTime? endDate, String tag);
        DeliveryErrorsResponse RetrieveDeliveryErrors(DateTime? startDate, DateTime? endDate, String tag);
        UnsubscribesResponse RetrieveUnsubscribes(DateTime? startDate, DateTime? endDate);
        MailingListCreateResponse CreateMailingList(MailingListCreateRequest mailingListCreateRequest);
        MailingListsResponse ListMailingLists();
        MailingListEntryCreateResponse CreateMailingListEntry(string mailingListKey, MailingListEntryCreateRequest mailingListEntryCreateRequest);
        MailingListEntryDeleteResponse DeleteMailingListEntry(string mailingListKey, string emailAddress);
    }
}