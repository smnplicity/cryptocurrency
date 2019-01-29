namespace CryptoCurrency.ExchangeClient.Bitfinex.WebSocket
{
    public class SubscriptionEventResponse : BaseEventResponse
    {        
        public string Channel { get; set; }

        public long ChanId { get; set; }
    }
}
