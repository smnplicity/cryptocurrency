using System.Collections.Concurrent;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using CryptoCurrency.Core.Historian;
using CryptoCurrency.Core.Historian.Model;

namespace CryptoCurrency.Repository.Logging
{
    public class HistorianLoggerProvider : ILoggerProvider
    {
        private IHistorianRepository HistorianRepository { get; set; }

        private ConcurrentQueue<HistorianLogItem> Queue { get; set; }

        public HistorianLoggerProvider(IHistorianRepository historianRepository)
        {
            HistorianRepository = historianRepository;

            BeginProcessQueue();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new HistorianLogger(this, categoryName);
        }

        public void Dispose()
        {
        }

        public void Enqueue(HistorianLogItem item)
        {
            Queue.Enqueue(item);
        }

        private void BeginProcessQueue() => Task.Run(async () =>
        {
            Queue = new ConcurrentQueue<HistorianLogItem>();

            while (true)
            {
                if (Queue.Count > 0)
                {
                    HistorianLogItem logItem;

                    if (Queue.TryDequeue(out logItem))
                        await HistorianRepository.WriteLog(logItem);
                }

                await Task.Delay(25);
            }
        });
    }
}
