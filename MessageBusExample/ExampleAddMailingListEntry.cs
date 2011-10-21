using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleAddMailingListEntry {

        private readonly IMessageBusMailingListClient MessageBus
            = MessageBusFactory.CreateMailingListClient("<YOUR API KEY>");

        void AddMailingListEntry(string mailListKey) {

            var entryToAdd = new MessageBusMailingListEntry();
            entryToAdd.MergeFields["%EMAIL%"] = "joe@example.com";
            entryToAdd.MergeFields["%NAME%"] = "Joe Soap";

            MessageBus.CreateMailingListEntry(mailListKey, entryToAdd);
        }
    }
}