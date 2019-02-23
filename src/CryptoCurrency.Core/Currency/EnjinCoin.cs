namespace CryptoCurrency.Core.Currency
{
    public class EnjinCoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.ENJ;
        
        public string Label => "EnjinCoin";

        public string Symbol => "ENJ";
    }
}
