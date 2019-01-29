using System;
using System.Collections.Generic;

namespace CryptoCurrency.Core.Interval.Group
{
    public class Month : IIntervalGroup
    {
        public IntervalGroupEnum IntervalGroup => IntervalGroupEnum.Month;

        public string SuffixKey => "M";

        public string SuffixLabel => "month";

        public string Label => "Month";

        public ICollection<int> SupportedDuration => new List<int> { 1 };
        
        public Interval GetInterval(IntervalKey intervalKey, Epoch epoch)
        {
            var startOfMonth = new DateTime(epoch.DateTime.Year, epoch.DateTime.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            return new Interval
            {
                IntervalKey = intervalKey,
                From = new Epoch(startOfMonth),
                To = new Epoch(startOfMonth.AddMonths(1))
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
