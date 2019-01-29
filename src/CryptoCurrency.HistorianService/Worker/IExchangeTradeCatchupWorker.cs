namespace CryptoCurrency.HistorianService.Worker
{
    public interface IExchangeTradeCatchupWorker
    {
        void Start(IExchangeWorker exchangeWorker, int limit);
    }
}
