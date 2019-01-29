namespace CryptoCurrency.Core.Currency
{
    public class Ethereum : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.ETH;

        public string Label => "Ethereum";

        public string Symbol => "Ξ";
    }
}