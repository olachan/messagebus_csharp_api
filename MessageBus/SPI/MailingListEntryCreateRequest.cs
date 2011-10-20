using System.Collections.Generic;

namespace MessageBus.SPI
{
    public class MailingListEntryCreateRequest
    {
        public MailingListEntryCreateRequest()
        {
            mergeFields = new List<Dictionary<string, string>>();
        }

        public List<Dictionary<string,string>> mergeFields { get; private set; }
    }
}