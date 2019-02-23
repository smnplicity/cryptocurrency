namespace CryptoCurrency.Core.Currency
{
    public class Stratis : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.STRAT;
        
        public string Label => "Stratis";

        public string Symbol => "STRAT";
    }
}
