using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleDeleteMailingListEntry {

        private readonly IMessageBusMailingListClient MessageBus
            = MessageBusFactory.CreateMailingListClient("<YOUR API KEY>");

        void DeleteMailingListEntry(string mailListKey, string emailToDelete) {
            MessageBus.DeleteMailingListEntry(mailListKey, emailToDelete);
        }
    }
}