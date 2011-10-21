using System;
using MessageBus.API;
using MessageBus.API.V3;

namespace MessageBusExample {
    public class ExampleMailingLists {

        private readonly IMessageBusMailingListClient MessageBus
            = MessageBusFactory.CreateMailingListClient("<YOUR API KEY>");

        void GetMailingLists() {

            var list = MessageBus.ListMailingLists();

            Console.WriteLine(String.Format("Mailing Lists Retrieved.  Record Count:{0}", list.Length));
        }
    }
}