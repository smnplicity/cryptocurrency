namespace CryptoCurrency.Core.Currency
{
    public class Dash : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.DASH;

        public string Label => "Dash";

        public string Symbol => "DASH";
    }
}