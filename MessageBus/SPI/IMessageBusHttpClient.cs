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
        DeliveryErrorsResponse RetrieveDeliveryErrors(DateTime? startDate, DateTime? endDate);
        UnsubscribesResponse RetrieveUnsubscribes(DateTime? startDate, DateTime? endDate);
        MailingListCreateResponse CreateMailingList(MailingListCreateRequest mailingListCreateRequest);
        MailingListsResponse ListMailingLists();
        MailingListEntryCreateRequest CreateMailingListEntry(string mailingListKey, MailingListEntryCreateRequest mailingListEntryCreateRequest);
        MailingListEntryDeleteResponse DeleteMailingListEntry(string mailingListKey, string emailAddress);
    }
}