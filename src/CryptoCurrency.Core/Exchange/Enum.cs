namespace CryptoCurrency.Core.Exchange
{
    public enum ExchangeEnum
    {
        Kraken = 3,
        Bitfinex = 6,
        CoinbasePro = 7,
        Binance = 10
    }

    public enum ExchangeStatsKeyEnum
    {
        OpenShorts = 2,
        OpenLongs = 3
    }

    public enum DepositStateEnum
    {
        Available = 0,
        Pending = 1
    }
}
