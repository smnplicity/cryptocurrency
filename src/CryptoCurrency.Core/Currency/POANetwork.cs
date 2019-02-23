namespace CryptoCurrency.Core.Currency
{
    public class POANetwork : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.POA;
        
        public string Label => "POANetwork";

        public string Symbol => "POA";
    }
}
