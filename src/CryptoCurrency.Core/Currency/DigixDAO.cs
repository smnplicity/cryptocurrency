namespace CryptoCurrency.Core.Currency
{
    public class DigixDAO : ICurrency
    {
        public CurrencyCodeEnum Code => CurrencyCodeEnum.DGD;
        
        public string Label => "DigixDAO";

        public string Symbol => "DGD";
    }
}
