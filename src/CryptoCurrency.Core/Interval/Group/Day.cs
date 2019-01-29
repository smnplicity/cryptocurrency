using System;
using System.Collections.Generic;

namespace CryptoCurrency.Core.Interval.Group
{
    public class Day : IIntervalGroup
    {
        public IntervalGroupEnum IntervalGroup => IntervalGroupEnum.Day;

        public string SuffixKey => "D";

        public string SuffixLabel => "day";

        public string Label => "Day";

        public ICollection<int> SupportedDuration => new List<int> { 1 };
        
        public Interval GetInterval(IntervalKey intervalKey, Epoch epoch)
        {
            var ts = TimeSpan.FromDays(intervalKey.Duration);

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
