namespace CryptoCurrency.Core.Currency
{
    public class Aeron : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.ARN;
        
        public string Label => "Aeron";

        public string Symbol => "ARN";
    }
}
