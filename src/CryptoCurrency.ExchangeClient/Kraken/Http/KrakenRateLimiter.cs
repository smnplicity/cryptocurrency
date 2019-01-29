using System.Threading.Tasks;

using CryptoCurrency.Core.RateLimiter;

namespace CryptoCurrency.ExchangeClient.Kraken.Http
{
    public class BinanceRateLimiter : IRateLimiter
    {
        public int Count { get; set; }

        private int MaxCount { get; set; }

        public BinanceRateLimiter()
        {
            Count = 0;

            MaxCount = 20;

            FillBucket();
        }

        private void FillBucket() => Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(1000);

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
