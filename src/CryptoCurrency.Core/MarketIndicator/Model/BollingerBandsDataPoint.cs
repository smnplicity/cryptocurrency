namespace CryptoCurrency.Core.MarketIndicator.Model
{
    public class BollingerBandsDataPoint : IndicatorDataPoint
    {
        public double Upper { get; set; }

        public double Middle { get; set; }

        public double Lower { get; set; }
    }
}
