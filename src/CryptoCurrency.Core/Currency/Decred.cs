namespace CryptoCurrency.Core.Currency
{
    public class Decred : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.DCR;
        
        public string Label => "Decred";

        public string Symbol => "DCR";
    }
}
