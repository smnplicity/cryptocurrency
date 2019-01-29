using System;
using System.Collections.Generic;

namespace CryptoCurrency.Core.Interval.Group
{
    public class Week : IIntervalGroup
    {
        public IntervalGroupEnum IntervalGroup => IntervalGroupEnum.Week;

        public string SuffixKey => "W";

        public string SuffixLabel => "week";

        public string Label => "Week";

        public ICollection<int> SupportedDuration => new List<int> { 1 };
        
        public Interval GetInterval(IntervalKey intervalKey, Epoch epoch)
        {
            var from = epoch.DateTime.Date.AddDays(-(int)epoch.DateTime.Date.DayOfWeek + (int)DayOfWeek.Monday);

            return new Interval
            {
                IntervalKey = intervalKey,
                From = new Epoch(from),
                To = new Epoch(from.AddDays(7))
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
