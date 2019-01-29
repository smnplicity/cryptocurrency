namespace CryptoCurrency.Core.Interval
{
    public class Interval
    {        
        public IntervalKey IntervalKey { get; set; }

        public Epoch From { get; set; }

        public Epoch To { get; set; }
    }
}