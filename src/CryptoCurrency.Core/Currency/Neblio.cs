namespace CryptoCurrency.Core.Currency
{
    public class Neblio : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.NEBL;
        
        public string Label => "Neblio";

        public string Symbol => "NEBL";
    }
}
