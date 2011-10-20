using System;
using MessageBus.SPI;

namespace MessageBus.API.V3 {
    public class MessageBusMailingList {

        public MessageBusMailingList() {

        }

        public MessageBusMailingList(MailingListItem item) {
            Key = item.key;
            Name = item.name;
        }

        public string Key { get; set; }
        public string Name { get; set; }
        public string[] MergeFieldKeys { get; set; }
    }
}