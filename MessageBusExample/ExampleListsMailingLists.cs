using System.IO;
using MessageBus.API;
using MessageBus.API.V3;
using MessageBus.Impl;

namespace MessageBusExample
{
    public class ExampleListsMailingLists {

        // replace with YOUR PRIVATE key, which can be found here: https://www.messagebus.com/api
        private readonly IMessageBusMailingListClient MessageBusMailingLists = MessageBusFactory.CreateMailingListClient("<YOUR API KEY>", new ConsoleLogger());

        /// <summary>
        /// This example uploads a mailing list and sends a campaign based on the mailing list
        /// </summary>
        MessageBusMailingList[] RunExample(string name, FileInfo mailingList) {

            try {

                return MessageBusMailingLists.ListMailingLists();

            } catch (MessageBusException) {
                throw;
            }
        }
    }
}