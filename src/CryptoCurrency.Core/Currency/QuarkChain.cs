namespace CryptoCurrency.Core.Currency
{
    public class QuarkChain : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.QKC;
        
        public string Label => "QuarkChain";

        public string Symbol => "QKC";
    }
}
