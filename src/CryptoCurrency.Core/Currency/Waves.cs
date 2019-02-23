namespace CryptoCurrency.Core.Currency
{
    public class Waves : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.WAVES;
        
        public string Label => "Waves";

        public string Symbol => "WAVES";
    }
}
