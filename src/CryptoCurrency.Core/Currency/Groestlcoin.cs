namespace CryptoCurrency.Core.Currency
{
    public class Groestlcoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.GRS;
        
        public string Label => "Groestlcoin";

        public string Symbol => "GRS";
    }
}
