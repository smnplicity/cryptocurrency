using System;
using System.Collections.Generic;

namespace CryptoCurrency.Core.Interval.Group
{
    public class Hour : IIntervalGroup
    {
        public IntervalGroupEnum IntervalGroup => IntervalGroupEnum.Hour;

        public string SuffixKey => "h";

        public string SuffixLabel => "hour";

        public string Label => "Hour";

        public ICollection<int> SupportedDuration => new List<int> { 1, 2, 3, 4, 6, 12 };
        
        public Interval GetInterval(IntervalKey intervalKey, Epoch epoch)
        {
            var ts = TimeSpan.FromHours(intervalKey.Duration);

            return new Interval
            {
                IntervalKey = intervalKey,
                From = epoch.RoundDown(ts),
                To = epoch.AddSeconds(1).RoundUp(ts)
            };
        }

        public Interval Next(Interval interval)
        {
            return GetInterval(interval.IntervalKey, interval.To);
        }

        public Interval Previous(Interval interval)
        {
            return GetInterval(interval.IntervalKey, interval.From.AddSeconds(-1));
        }
    }
}
