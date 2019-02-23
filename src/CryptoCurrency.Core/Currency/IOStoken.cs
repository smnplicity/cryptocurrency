namespace CryptoCurrency.Core.Currency
{
    public class IOStoken : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.IOST;
        
        public string Label => "IOStoken";

        public string Symbol => "IOST";
    }
}
