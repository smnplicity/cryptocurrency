namespace CryptoCurrency.Core.Currency
{
    public class Syscoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.SYS;
        
        public string Label => "Syscoin";

        public string Symbol => "SYS";
    }
}
