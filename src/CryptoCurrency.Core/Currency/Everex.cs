namespace CryptoCurrency.Core.Currency
{
    public class Everex : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.EVX;
        
        public string Label => "Everex";

        public string Symbol => "EVX";
    }
}
