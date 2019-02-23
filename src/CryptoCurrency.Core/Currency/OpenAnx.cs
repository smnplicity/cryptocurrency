namespace CryptoCurrency.Core.Currency
{
    public class OpenAnx : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.OAX;
        
        public string Label => "OpenAnx";

        public string Symbol => "OAX";
    }
}
