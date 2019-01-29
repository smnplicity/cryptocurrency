using System.Collections.Generic;

namespace CryptoCurrency.Core.Interval
{
    public interface IIntervalFactory
    {
        IIntervalGroup GetGroup(IntervalGroupEnum group);

        ICollection<IIntervalGroup> ListGroups();

        ICollection<IntervalKey> ListIntervalKeys(IntervalGroupEnum intervalGroup);

        IntervalKey GetIntervalKey(string intervalKey);
               
        ICollection<Interval> GenerateIntervals(IntervalKey intervalKey, Epoch from, Epoch to);

        ICollection<Interval> GenerateIntervals(IntervalKey intervalKey, Epoch to, int intervalCount);

        Interval GetInterval(IntervalKey intervalKey, Epoch epoch, int offset = 0);
    }
}
