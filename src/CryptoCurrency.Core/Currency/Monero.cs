namespace CryptoCurrency.Core.Currency
{
    public class Monero : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.XMR;
        
        public string Label => "Monero";

        public string Symbol => "XMR";
    }
}
