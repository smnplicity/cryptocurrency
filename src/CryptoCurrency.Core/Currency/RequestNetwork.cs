namespace CryptoCurrency.Core.Currency
{
    public class RequestNetwork : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.REQ;
        
        public string Label => "RequestNetwork";

        public string Symbol => "REQ";
    }
}
