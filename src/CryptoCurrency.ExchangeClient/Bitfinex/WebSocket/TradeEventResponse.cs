
namespace CryptoCurrency.ExchangeClient.Bitfinex.WebSocket
{
    public class TradeEventResponse : SubscriptionEventResponse
    {
        public string Symbol { get; set; }

        public string Pair { get; set; }
    }
}
