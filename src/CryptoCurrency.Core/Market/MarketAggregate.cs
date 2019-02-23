namespace CryptoCurrency.Core.Market
{
    public class MarketAggregate : Ohlc
    {
        public double? BuyVolume { get; set; }

        public double? SellVolume { get; set; }

        public double? TotalVolume { get; set; }

        public int? BuyCount { get; set; }

        public int? SellCount { get; set; }

        public int TotalCount { get; set; }
    }
}
