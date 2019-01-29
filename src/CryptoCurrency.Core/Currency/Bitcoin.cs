namespace CryptoCurrency.Core.Currency
{
    public class Bitcoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.BTC;
        
        public string Label => "Bitcoin";

        public string Symbol => "Ƀ";
    }
}
