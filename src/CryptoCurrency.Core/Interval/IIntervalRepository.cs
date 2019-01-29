using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoCurrency.Core.Interval
{
    public interface IIntervalRepository
    {
        Task Add(IntervalKey intervalKey);

        Task AddInterval(ICollection<Interval> interval);
    }
}
