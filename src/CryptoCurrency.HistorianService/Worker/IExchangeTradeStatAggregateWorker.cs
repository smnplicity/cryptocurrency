namespace CryptoCurrency.HistorianService.Worker
{
    public interface IExchangeTradeStatAggregateWorker
    {
        void Start(IExchangeWorker exchangeWorker, int pageSize);
    }
}
