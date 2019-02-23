namespace CryptoCurrency.Core.Currency
{
    public class Viacoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.VIA;
        
        public string Label => "Viacoin";

        public string Symbol => "VIA";
    }
}
