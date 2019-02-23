namespace CryptoCurrency.Core.Currency
{
    public class Mainframe : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.MFT;
        
        public string Label => "Mainframe";

        public string Symbol => "MFT";
    }
}
