namespace CryptoCurrency.Core.Currency
{
    public class Tether : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.USDT;
        
        public string Label => "Tether";

        public string Symbol => "₮";
    }
}