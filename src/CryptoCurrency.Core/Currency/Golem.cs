namespace CryptoCurrency.Core.Currency
{
    public class Golem : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.GNT;
        
        public string Label => "Golem";

        public string Symbol => "GNT";
    }
}
