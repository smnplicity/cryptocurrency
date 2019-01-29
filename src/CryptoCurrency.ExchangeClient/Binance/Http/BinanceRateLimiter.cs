using System.Threading.Tasks;

using CryptoCurrency.Core.RateLimiter;

namespace CryptoCurrency.ExchangeClient.Binance.Http
{
    public class CoinbaseProRateLimiter : IRateLimiter
    {
        public int Count { get; set; }

        private int MaxCount { get; set; }

        public CoinbaseProRateLimiter()
        {
            Count = 0;

            MaxCount = 20;

            FillBucket();
        }

        private void FillBucket() => Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(3000);

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
