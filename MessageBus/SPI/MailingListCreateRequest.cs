using System;
using MessageBus.API.V3;

namespace MessageBus.SPI
{
    public class MailingListCreateRequest
    {
        public MailingListCreateRequest(MessageBusMailingList mailingList)
        {
            name = mailingList.Name;
            mergeFieldKeys = mailingList.MergeFieldKeys;
        }

        public string name { get; set; }
        public string[] mergeFieldKeys { get; set; }
    }
}