namespace CryptoCurrency.HistorianService.Worker
{
    public interface IExchangeTradeAggregateWorker
    {
        void Start(IExchangeWorker exchangeWorker, int pageSize);
    }
}