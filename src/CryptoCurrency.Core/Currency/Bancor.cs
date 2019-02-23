namespace CryptoCurrency.Core.Currency
{
    public class Bancor : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.BNT;
        
        public string Label => "Bancor";

        public string Symbol => "BNT";
    }
}
