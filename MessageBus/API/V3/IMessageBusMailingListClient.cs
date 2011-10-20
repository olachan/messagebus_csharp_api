namespace MessageBus.API.V3 {
    public interface IMessageBusMailingListClient {
        MessageBusMailingList CreateMailingList(MessageBusMailingList mailingList);
        MessageBusMailingList[] ListMailingLists();
        void CreateMailingListEntry(string mailingListKey, MessageBusMailingListEntry entry);
        void DeleteMailingListEntry(string mailingListKey, string emailAddress);
    }
}