namespace CryptoCurrency.Core.Market
{
    public class MarketAggregate : Ohlc
    {
        public decimal? BuyVolume { get; set; }

        public decimal? SellVolume { get; set; }

        public decimal? TotalVolume { get; set; }

        public int? BuyCount { get; set; }

        public int? SellCount { get; set; }

        public int TotalCount { get; set; }
    }
}
