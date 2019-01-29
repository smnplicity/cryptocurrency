namespace CryptoCurrency.HistorianService.Worker
{
    public interface IExchangeTradeStatWorker
    {
        void Start(IExchangeWorker exchangeWorker);
    }
}
