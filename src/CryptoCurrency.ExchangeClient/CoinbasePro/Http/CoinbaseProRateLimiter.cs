using System.Threading.Tasks;

using CryptoCurrency.Core.RateLimiter;

namespace CryptoCurrency.ExchangeClient.CoinbasePro.Http
{
    public class CoinbaseProRateLimiter : IRateLimiter
    {
        public int Count { get; set; }

        private int MaxCount { get; set; }

        public CoinbaseProRateLimiter()
        {
            Count = 0;

            MaxCount = 3;

            FillBucket();
        }

        private void FillBucket() => Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(500);

                if (Count > 0)
                    Count--;
            }
        });

        public async Task Wait()
        {
            while (true)
            {
                if (Count + 1 < MaxCount)
                {
                    Count++;

                    return;
                }

                await Task.Delay(5);
            }
        }
    }
}
