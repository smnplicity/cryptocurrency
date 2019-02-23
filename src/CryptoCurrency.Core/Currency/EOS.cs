namespace CryptoCurrency.Core.Currency
{
    public class EOS : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.EOS;
        
        public string Label => "EOS";

        public string Symbol => "EOS";
    }
}
