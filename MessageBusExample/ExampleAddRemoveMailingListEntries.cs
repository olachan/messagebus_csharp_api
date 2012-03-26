using System.IO;
using MessageBus.API;
using MessageBus.API.V3;
using MessageBus.Impl;

namespace MessageBusExample
{
    public class ExampleAddRemoveMailingListEntries {

        // replace with YOUR PRIVATE key, which can be found here: https://www.messagebus.com/api
        private readonly IMessageBusMailingListClient MessageBusMailingLists = MessageBusFactory.CreateMailingListClient("<YOUR API KEY>", new ConsoleLogger());

        /// <summary>
        /// This example uploads a mailing list and sends a campaign based on the mailing list
        /// </summary>
        void RunExample(string name, FileInfo mailingList) {

            try {

                var uploadResult = MessageBusMailingLists.UploadMailingList(name, mailingList);

                var mailingListKey = uploadResult.MailingListKey;

                var newEntry = new MessageBusMailingListEntry();
                newEntry.MergeFields["%EMAIL%"] = "bob@example.com";
                newEntry.MergeFields["%FIRST_NAME%"] = "Bob";

                MessageBusMailingLists.AddMailingListEntry(mailingListKey, newEntry);

                MessageBusMailingLists.DeleteMailingListEntry(mailingListKey, "bob@example.com");

            } catch (MessageBusException) {
                throw;
            }
        }
    }
}