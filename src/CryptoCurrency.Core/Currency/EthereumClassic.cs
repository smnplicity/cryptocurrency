namespace CryptoCurrency.Core.Currency
{
    public class EthereumClassic : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.ETC;
        
        public string Label => "Ethereum Classic";

        public string Symbol => "ETH";
    }
}