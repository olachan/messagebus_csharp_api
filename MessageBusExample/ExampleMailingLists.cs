using System;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleMailingLists {

        private readonly IMessageBusMailingListClient MessageBus = MessageBusFactory.CreateMailingListClient("<YOUR API KEY>");

        void RunExample() {
            try {

                // first create a new empty mailing list

                var list = MessageBus.CreateMailingList(new MessageBusMailingList {
                    Name = "example mailing list",
                    MergeFieldKeys =
                        new[] { "%EMAIL", "%FIRST_NAME%", "%LAST_NAME%" }
                });

                Console.WriteLine(String.Format("A mailing list with key {0} was created", list.Key));

                // next add 2 new entries to the list

                var entry1 = new MessageBusMailingListEntry();
                entry1.MergeFields["%EMAIL%"] = "jane@example.com";
                entry1.MergeFields["%FIRST_NAME%"] = "Jane";
                entry1.MergeFields["%LAST_NAME%"] = "Smith";

                MessageBus.CreateMailingListEntry(list.Key, entry1);

                var entry2 = new MessageBusMailingListEntry();
                entry2.MergeFields["%EMAIL%"] = "john@example.com";
                entry2.MergeFields["%FIRST_NAME%"] = "John";
                entry2.MergeFields["%LAST_NAME%"] = "Smith";

                MessageBus.CreateMailingListEntry(list.Key, entry2);

                // next delete one of the entries

                MessageBus.DeleteMailingListEntry(list.Key, "john@example.com");

                // finnally list all the mailing lists

                var lists = MessageBus.ListMailingLists();

                Console.WriteLine("You have the following mailing lists: ");
                foreach (var item in lists) {
                    Console.WriteLine(String.Format("A list named {0} with key {1}", item.Name, item.Key));
                }

            } catch (MessageBusException e) {
                throw;
            }
        }
    }
}