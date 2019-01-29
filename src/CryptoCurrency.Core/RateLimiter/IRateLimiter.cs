using System.Threading.Tasks;

namespace CryptoCurrency.Core.RateLimiter
{
    public interface IRateLimiter
    {
        int Count { get; set; }

        Task Wait();
    }
}
