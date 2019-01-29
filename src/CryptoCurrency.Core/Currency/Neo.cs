namespace CryptoCurrency.Core.Currency
{
    public class Neo : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.NEO;

        public string Label => "Neo";

        public string Symbol => "NEO";
    }
}