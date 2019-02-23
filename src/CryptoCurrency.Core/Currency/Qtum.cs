namespace CryptoCurrency.Core.Currency
{
    public class Qtum : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.QTUM;
        
        public string Label => "Qtum";

        public string Symbol => "QTUM";
    }
}
