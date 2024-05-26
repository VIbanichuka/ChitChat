namespace ChitChat.App.Server.Models
{
    public class HubModel
    {
        public string? ChannelName { get; set; }
        public string? Sender { get; set; }
        public string? Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
