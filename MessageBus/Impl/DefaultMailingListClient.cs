using System;
using System.Linq;
using System.Net;
using MessageBus.API;
using MessageBus.API.V3;
using MessageBus.API.V3.Debug;
using MessageBus.SPI;

namespace MessageBus.Impl {
    public class DefaultMailingListClient : IMessageBusMailingListClient, IMessageBusDebugging {

        private readonly ILogger Logger;
        private readonly IMessageBusHttpClient HttpClient;

        public DefaultMailingListClient(string apiKey) {
            HttpClient = new SimpleHttpClient(apiKey);
            Logger = new NullLogger();
        }

        public DefaultMailingListClient(string apiKey, ILogger logger) {
            HttpClient = new SimpleHttpClient(apiKey);
            Logger = logger;
        }

        public DefaultMailingListClient(IMessageBusHttpClient httpClient, ILogger logger) {
            HttpClient = httpClient;
            Logger = logger;
        }

        public MessageBusMailingList CreateMailingList(MessageBusMailingList mailingList) {
            var response = HttpClient.CreateMailingList(new MailingListCreateRequest(mailingList));
            if (response.statusCode != 201) {
                throw new MessageBusException(response.statusCode, response.statusMessage);
            }
            mailingList.Key = response.key;
            return mailingList;
        }

        public MessageBusMailingList[] ListMailingLists() {
            var response = HttpClient.ListMailingLists();
            if (response.statusCode != 200) {
                throw new MessageBusException(response.statusCode, response.statusMessage);
            }
            return response.results.Select(r => new MessageBusMailingList(r)).ToArray();
        }

        public void CreateMailingListEntry(string mailingListKey, MessageBusMailingListEntry entry) {
            var response = HttpClient.CreateMailingListEntry(mailingListKey, new MailingListEntryCreateRequest(entry));
            if (response.statusCode != 201) {
                throw new MessageBusException(response.statusCode, response.statusMessage);
            }
        }

        public void DeleteMailingListEntry(string mailingListKey, string emailAddress) {
            var response = HttpClient.DeleteMailingListEntry(mailingListKey, emailAddress);
            if (response.statusCode != 200) {
                throw new MessageBusException(response.statusCode, response.statusMessage);
            }
        }

        public bool SkipValidation { private get; set; }


        public string Domain {
            set { HttpClient.Domain = value; }
        }

        public string Path {
            set { HttpClient.Path = value; }
        }

        public bool SslVerifyPeer {
            set { HttpClient.SslVerifyPeer = value; }
        }

        public IWebProxy Proxy {
            set { HttpClient.Proxy = value; }
        }
    }
}