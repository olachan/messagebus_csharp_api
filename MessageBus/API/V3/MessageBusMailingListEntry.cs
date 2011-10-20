using System.Collections.Generic;

namespace MessageBus.API.V3
{
    public class MessageBusMailingListEntry
    {
        public MessageBusMailingListEntry()
        {
            MergeFields = new Dictionary<string, string>();
        }

        public Dictionary<string,string> MergeFields { get; private set; }
    }
}