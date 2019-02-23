namespace CryptoCurrency.Core.Currency
{
    public class Komodo : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.KMD;
        
        public string Label => "Komodo";

        public string Symbol => "KMD";
    }
}
