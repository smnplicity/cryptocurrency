namespace CryptoCurrency.Core.Currency
{
    public class Siacoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.SC;
        
        public string Label => "Siacoin";

        public string Symbol => "SC";
    }
}
