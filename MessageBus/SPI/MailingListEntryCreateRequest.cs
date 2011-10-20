using System;
using System.Collections.Generic;
using MessageBus.API.V3;

namespace MessageBus.SPI
{
    public class MailingListEntryCreateRequest
    {
        public MailingListEntryCreateRequest(MessageBusMailingListEntry entry)
        {
            mergeFields = entry.MergeFields;
        }

        public Dictionary<string,string> mergeFields { get; private set; }
    }
}