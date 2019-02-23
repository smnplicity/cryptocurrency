namespace CryptoCurrency.Core.Currency
{
    public class SimpleToken : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.OST;
        
        public string Label => "SimpleToken";

        public string Symbol => "OST";
    }
}
