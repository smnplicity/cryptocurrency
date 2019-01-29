namespace CryptoCurrency.HistorianService.Worker
{
    public interface IExchangeTradeWorker
    {
        void Start(IExchangeWorker exchangeWorker, int limit);
    }
}