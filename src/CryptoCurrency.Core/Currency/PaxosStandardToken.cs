namespace CryptoCurrency.Core.Currency
{
    public class PaxosStandardToken : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.PAX;
        
        public string Label => "PaxosStandardToken";

        public string Symbol => "PAX";
    }
}
