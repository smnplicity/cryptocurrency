
namespace CryptoCurrency.ExchangeClient.Bitfinex.WebSocket
{
    public class TickerEventResponse : SubscriptionEventResponse
    {
        public string Symbol { get; set; }

        public string Pair { get; set; }
    }
}
