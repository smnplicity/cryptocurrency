namespace CryptoCurrency.Core.Currency
{
    public class BinanceCoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.BNB;

        public string Label => "Binance Coin";

        public string Symbol => "BNB";
    }
}
