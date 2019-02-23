namespace CryptoCurrency.Core.Currency
{
    public class Cardano : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.ADA;
        
        public string Label => "Cardano";

        public string Symbol => "ADA";
    }
}
