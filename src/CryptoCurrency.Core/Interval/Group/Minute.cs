using System;
using System.Collections.Generic;

namespace CryptoCurrency.Core.Interval.Group
{
    public class Minute : IIntervalGroup
    {
        public IntervalGroupEnum IntervalGroup => IntervalGroupEnum.Minute;

        public string SuffixKey => "m";

        public string SuffixLabel => "minute";

        public string Label => "Minute";

        public ICollection<int> SupportedDuration => new List<int> { 1, 3, 5, 15, 30 };

        public Interval GetInterval(IntervalKey intervalKey, Epoch epoch)
        {
            var ts = TimeSpan.FromMinutes(intervalKey.Duration);

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
