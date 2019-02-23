namespace CryptoCurrency.Core.Currency
{
    public class TRON : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.TRX;
        
        public string Label => "TRON";

        public string Symbol => "TRX";
    }
}
