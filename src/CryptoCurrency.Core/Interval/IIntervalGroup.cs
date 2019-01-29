using System.Collections.Generic;

namespace CryptoCurrency.Core.Interval
{
    public interface IIntervalGroup
    {
        IntervalGroupEnum IntervalGroup { get; }

        string SuffixKey { get; }

        string SuffixLabel { get; }

        string Label { get; }

        ICollection<int> SupportedDuration { get; }

        Interval GetInterval(IntervalKey intervalKey, Epoch epoch);

        Interval Next(Interval interval);

        Interval Previous(Interval interval);
    }
}
