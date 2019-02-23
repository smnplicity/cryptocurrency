namespace CryptoCurrency.Core.Currency
{
    public class Ravencoin : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.RVN;
        
        public string Label => "Ravencoin";

        public string Symbol => "RVN";
    }
}
