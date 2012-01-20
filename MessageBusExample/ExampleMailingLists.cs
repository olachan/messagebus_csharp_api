// Copyright (c) 2012. Mail Bypass, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License
//

// This example demonstrates various api methods relating to mailing list management.
// We create a new blank mailing list; add two entries to the new list; delete one of
// the entries; and then retrieve the list of all mailing lists

using System;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleMailingLists {

        // replace with YOUR PRIVATE key, which can be found here: https://www.messagebus.com/api
        private readonly IMessageBusMailingListClient MessageBus = MessageBusFactory.CreateMailingListClient("<YOUR API KEY>");

        /// <summary>
        /// This example demonstrates various api methods relating to mailing list management.
        /// We create a new blank mailing list; add two entries to the new list; delete one of
        /// the entries; and then retrieve the list of all mailing lists
        /// </summary>
        void RunExample() {
            try {

                // first create a new blank mailing list

                var list = MessageBus.CreateMailingList(new MessageBusMailingList {
                    Name = "example mailing list",
                    MergeFieldKeys =
                        new[] { "%EMAIL", "%FIRST_NAME%", "%LAST_NAME%" }
                });

                Console.WriteLine(String.Format("A mailing list with key {0} was created", list.Key));

                // after the new mailing list is created, add two entries to the list,

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

                // having added two entries, delete one of them

                MessageBus.DeleteMailingListEntry(list.Key, "john@example.com");

                // finally, list all the mailing lists

                var lists = MessageBus.ListMailingLists();

                Console.WriteLine("You have the following mailing lists: ");
                foreach (var item in lists) {
                    Console.WriteLine(String.Format("A list named {0} with key {1}", item.Name, item.Key));
                }

            } catch (MessageBusException) {
                throw;
            }
        }
    }
}