namespace MessageBus.SPI
{
    public class MailingListCreateRequest
    {
        public string name { get; set; }
        public string[] mergeFieldKeys { get; set; }
    }
}