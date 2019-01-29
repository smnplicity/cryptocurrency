namespace CryptoCurrency.Core.MarketIndicator.Model
{
    public class MacdDataPoint : IndicatorDataPoint
    {
        public double Macd { get; set; }

        public double Signal { get; set; }

        public double Histogram { get; set; }
    }
}
